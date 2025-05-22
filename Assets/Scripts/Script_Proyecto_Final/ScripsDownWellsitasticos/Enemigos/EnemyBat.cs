// EnemyBat.cs
using UnityEngine;

public class EnemyBat : MonoBehaviour
{
    public enum BatState { Sleeping, Patrolling, Chasing }

    [Header("Estado Inicial")]
    public BatState currentState = BatState.Sleeping;

    [Header("Movimiento y Persecuci�n")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4.5f;
    public float detectionRange = 6f; // Rango para despertar/empezar a perseguir
    public float loseAgroRangeFactor = 1.5f; // Multiplicador de detectionRange para dejar de perseguir
    [Tooltip("Qu� tan r�pido el murci�lago ajusta su direcci�n hacia el jugador al perseguir. Valores m�s altos = m�s �gil.")]
    public float chaseIntensity = 5f; // Factor de Lerp para la velocidad de persecuci�n (agresividad)
    public LayerMask wallLayer; // Layer para los objetos que son paredes/suelo/techo

    [Header("Combate")]
    public int damageToPlayer = 1; // Da�o que hace el murci�lago al jugador

    [Header("Referencias (Opcional si se auto-asignan)")]
    public Transform playerTransform; // Asignar el jugador en el Inspector o se buscar� por Tag

    // Componentes y variables internas
    private Rigidbody2D rb;
    private Vector2 movementDirection = Vector2.right; // Direcci�n inicial de patrulla
    private bool isFacingRight = true;
    private Vector2 initialPosition; // Posici�n donde "duerme" o inicia patrulla

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("EnemyBat requiere un componente Rigidbody2D.", this);
            enabled = false;
            return;
        }
    }

    void Start()
    {
        initialPosition = transform.position;

        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
            else
            {
                Debug.LogError("Jugador no encontrado. Asigna 'playerTransform' en el Inspector del EnemyBat o aseg�rate que el jugador tenga el tag 'Player'.", this);
                enabled = false; // Deshabilitar script si no hay jugador
                return;
            }
        }

        // Configurar estado inicial y sprite
        InitializeBatDirection();
        SetupInitialState();
    }

    void InitializeBatDirection()
    {
        // Asegurar que el sprite mire en la direcci�n de 'movementDirection'
        if (movementDirection.x > 0) isFacingRight = true;
        else if (movementDirection.x < 0) isFacingRight = false;

        if ((isFacingRight && transform.localScale.x < 0) || (!isFacingRight && transform.localScale.x > 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    void SetupInitialState()
    {
        switch (currentState)
        {
            case BatState.Sleeping:
                rb.bodyType = RigidbodyType2D.Kinematic; // No afectado por f�sica mientras duerme
                rb.velocity = Vector2.zero;
                break;
            case BatState.Patrolling:
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 0; // Para que vuele
                break;
            case BatState.Chasing: // Estado inicial poco com�n, pero posible
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 0;
                break;
        }
    }


    void Update() // L�gica de estados y detecci�n
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case BatState.Sleeping:
                if (distanceToPlayer < detectionRange)
                {
                    WakeUp();
                }
                break;
            case BatState.Patrolling:
                DetectWalls();
                if (distanceToPlayer < detectionRange)
                {
                    currentState = BatState.Chasing;
                    Debug.Log("Murci�lago: Cambiando a estado Chasing");
                }
                break;
            case BatState.Chasing:
                if (distanceToPlayer > detectionRange * loseAgroRangeFactor)
                {
                    currentState = BatState.Patrolling; // O volver a dormir: GoToSleep();
                    Debug.Log("Murci�lago: Perdi� al jugador, volviendo a Patrullar");
                }
                break;
        }
    }

    void FixedUpdate() // Movimiento basado en f�sicas
    {
        if (playerTransform == null || rb.bodyType == RigidbodyType2D.Kinematic) return;

        switch (currentState)
        {
            case BatState.Patrolling:
                PatrolMovement();
                break;
            case BatState.Chasing:
                ChasePlayerMovement();
                break;
        }
    }

    void WakeUp()
    {
        Debug.Log("Murci�lago: �Despertando!");
        currentState = BatState.Chasing;
        rb.bodyType = RigidbodyType2D.Dynamic; // Hacerlo din�mico para moverse
        rb.gravityScale = 0; // Asegurar que vuele
    }

    void DetectWalls()
    {
        // Raycast para detectar paredes. Ajustar la longitud del rayo (0.6f) y el offset si es necesario.
        // El offset vertical (Vector2.up * 0.1f) es por si el pivote del murci�lago est� en su base.
        float raycastOriginOffset = rb.GetComponent<Collider2D>() != null ? rb.GetComponent<Collider2D>().bounds.extents.x : 0.5f;
        RaycastHit2D wallHit = Physics2D.Raycast(rb.position, movementDirection, raycastOriginOffset + 0.1f, wallLayer);
        Debug.DrawRay(rb.position, movementDirection * (raycastOriginOffset + 0.1f), Color.red);

        if (wallHit.collider != null)
        {
            FlipPatrolDirection();
        }
    }

    void PatrolMovement()
    {
        rb.velocity = movementDirection * patrolSpeed;
    }

    void ChasePlayerMovement()
    {
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - rb.position).normalized;
        Vector2 targetVelocity = directionToPlayer * chaseSpeed;

        // Interpolar la velocidad actual hacia la velocidad objetivo para un movimiento m�s suave
        // chaseIntensity controla qu� tan r�pido se ajusta. Time.fixedDeltaTime es importante en FixedUpdate.
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, chaseIntensity * Time.fixedDeltaTime);

        // Girar el sprite para encarar al jugador basado en la velocidad
        if (rb.velocity.x > 0.05f && !isFacingRight)
        {
            ForceFlipSprite(true); // Mirar a la derecha
        }
        else if (rb.velocity.x < -0.05f && isFacingRight)
        {
            ForceFlipSprite(false); // Mirar a la izquierda
        }
    }

    void FlipPatrolDirection() // Usado para patrullar
    {
        movementDirection *= -1;
        isFacingRight = !isFacingRight;
        FlipSpriteScale();
        Debug.Log("Murci�lago: Choc� con pared, girando.");
    }

    void ForceFlipSprite(bool faceRight) // Usado para persecuci�n o establecer direcci�n
    {
        if (isFacingRight != faceRight)
        {
            isFacingRight = faceRight;
            FlipSpriteScale();
        }
    }

    void FlipSpriteScale() // Invierte la escala local en X para girar el sprite
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void TakeDamageFromStomp()
    {
        Debug.Log("Enemigo Murci�lago: �Recib� da�o por arriba! (Stomp)");
        Die();
    }

    public void DealDamageToPlayer(GameObject playerObject)
    {
        PlayerHealth playerHealth = playerObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Enemigo Murci�lago: �Hice da�o al jugador!");
            playerHealth.GetHitByEnemy(damageToPlayer);
        }
    }

    void Die()
    {
        Debug.Log("Enemigo Murci�lago: Enemigo muerto.");
        // Aqu� puedes a�adir efectos de part�culas, sonido, etc.
        Destroy(gameObject);
    }

    // Opcional: si prefieres usar OnCollisionEnter2D para paredes en lugar de Raycast
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (currentState == BatState.Patrolling)
    //     {
    //         if (((1 << collision.gameObject.layer) & wallLayer) != 0) // Comprueba si colision� con la layer "Wall"
    //         {
    //             FlipPatrolDirection();
    //         }
    //     }
    // }
}