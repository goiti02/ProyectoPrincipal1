using UnityEngine;
using System.Collections; // Necesario para Coroutines

[RequireComponent(typeof(Rigidbody2D))]
public class Ametralladora : MonoBehaviour
{
    // --- Cláse interna para el comportamiento de la bala ---
    // Esta clase se añadirá a cada bala que se instancia.
    private class ComportamientoBala : MonoBehaviour
    {
        // Tags con los que la bala debe colisionar y destruirse
        private readonly string[] tagsColision = { "SueloDestruible", "Enemigo", "Suelo" };

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Comprobamos si el tag del objeto con el que colisionamos está en nuestra lista
            bool chocadoConAlgoSolido = false;
            foreach (string tag in tagsColision)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    chocadoConAlgoSolido = true;
                    break; // Salimos del bucle si encontramos una coincidencia
                }
            }

            if (chocadoConAlgoSolido)
            {
                // Podrías añadir lógica aquí antes de destruir, como aplicar daño si es "Enemigo"
                // if (collision.gameObject.CompareTag("Enemigo")) {
                //     // ... código para hacer daño ...
                // }

                Destroy(gameObject); // Destruye ESTA bala (el GameObject al que este script está adjunto)
            }
            // Nota: Si choca con algo que no está en la lista (ej: otra bala, powerup), no se destruye aquí.
        }

        // Puedes añadir más lógica específica de la bala aquí si es necesario
        // void Start() { }
        // void Update() { }
    }
    // --- Fin de la clase interna ---


    [Header("Referencias")]
    public Transform firePoint;          // Punto de origen del disparo
    public GameObject bulletPrefab;      // Prefab de la bala (¡Debe tener Rigidbody2D y Collider2D!)
    public Transform groundCheck;        // Objeto para detectar el suelo

    [Header("Parámetros del Arma")]
    public float velocidadDeDisparo = 0.5f; // Tiempo mínimo ENTRE ráfagas (segundos)
    public float tiempoDeVidaBala = 1.5f;  // Tiempo MÁXIMO de vida si no choca (segundos)
    // public float dañoDisparo = 1f; // El daño debería estar en la bala o pasarse al instanciar/colisionar
    public int disparosMaximosAire = 15; // Número de disparos posibles antes de tocar el suelo
    public float fuerzaEmpujeDisparo = 3f; // Fuerza hacia arriba al disparar en el aire

    [Header("Parámetros de Ráfaga")]
    public int proyectilesPorRafaga = 3;   // Número de proyectiles por ráfaga
    public float retrasoEntreBalasRafaga = 0.08f; // Tiempo entre balas DENTRO de una ráfaga (segundos)

    [Header("Parámetros de la Bala")]
    public float velocidadBala = 12f;    // Velocidad del proyectil

    [Header("Chequeo de Suelo")]
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    // Estado interno
    private Rigidbody2D playerRb;
    private int balasRestantesEnAire;
    private bool isGrounded;
    private bool puedeDisparar = true; // Controla el cooldown entre ráfagas
    private bool tocandoSueloPrevio = true; // Para detectar cuándo se toca el suelo por primera vez

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogError("El script Ametralladora debe estar adjunto a un GameObject con un Rigidbody2D.", this);
            enabled = false;
            return;
        }

        if (firePoint == null || bulletPrefab == null || groundCheck == null)
        {
            Debug.LogError("¡Asigna Fire Point, Bullet Prefab y Ground Check en el Inspector!", this);
            enabled = false;
            return;
        }
        // Validación importante: El prefab de la bala NECESITA un Collider y un Rigidbody
        if (bulletPrefab.GetComponent<Collider2D>() == null || bulletPrefab.GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError("¡El 'bulletPrefab' DEBE tener componentes Collider2D y Rigidbody2D!", this);
            enabled = false;
            return;
        }
        // Asegúrate de que el Rigidbody de la bala sea Kinematic o tenga gravedad 0 si no quieres que caiga por sí sola
        // Rigidbody2D rbPrefab = bulletPrefab.GetComponent<Rigidbody2D>();
        // rbPrefab.gravityScale = 0; // Ejemplo


        balasRestantesEnAire = disparosMaximosAire;
        ActualizarContadorBalas(true);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (isGrounded)
        {
            if (!tocandoSueloPrevio)
            {
                Debug.Log("¡Tocado el suelo!");
                if (balasRestantesEnAire < disparosMaximosAire)
                {
                    RecargarBalas();
                }
                tocandoSueloPrevio = true;
            }
        }
        else
        {
            tocandoSueloPrevio = false;
        }

        if (Input.GetButtonDown("Fire1") && puedeDisparar && balasRestantesEnAire > 0)
        {
            StartCoroutine(DispararRafaga());
        }
        else if (Input.GetButtonDown("Fire1") && balasRestantesEnAire <= 0)
        {
            Debug.Log("¡Ametralladora sin balas en el aire!");
        }
    }

    IEnumerator DispararRafaga()
    {
        puedeDisparar = false;

        for (int i = 0; i < proyectilesPorRafaga; i++)
        {
            if (balasRestantesEnAire <= 0)
            {
                Debug.Log("¡Ametralladora sin balas en el aire durante ráfaga!");
                break;
            }

            // --- Disparar una bala ---
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>(); // Ya validamos que existe en Start()

            // Establecer velocidad
            rbBullet.velocity = -firePoint.up * velocidadBala;

            // *** ¡AÑADIR EL COMPONENTE DE COMPORTAMIENTO A LA BALA! ***
            bullet.AddComponent<ComportamientoBala>();
            // Asignar el tag "Bala" a la instancia para que SueloDestruible funcione
            bullet.tag = "Bala"; // Asegúrate de que el tag "Bala" existe en el proyecto

            // Empujar al jugador
            if (!isGrounded && playerRb != null)
            {
                playerRb.AddForce(firePoint.up * fuerzaEmpujeDisparo, ForceMode2D.Impulse);
            }

            balasRestantesEnAire--;
            ActualizarContadorBalas(false);

            // Destruir la bala después de un tiempo MÁXIMO (si no colisiona antes)
            Destroy(bullet, tiempoDeVidaBala);
            // --- Fin Disparar una bala ---

            if (i < proyectilesPorRafaga - 1)
            {
                yield return new WaitForSeconds(retrasoEntreBalasRafaga);
            }
        }

        yield return new WaitForSeconds(velocidadDeDisparo);
        puedeDisparar = true;
    }

    void RecargarBalas()
    {
        if (balasRestantesEnAire < disparosMaximosAire)
        {
            balasRestantesEnAire = disparosMaximosAire;
            Debug.Log("¡Ametralladora Recargada!");
            ActualizarContadorBalas(true);
        }
    }

    void ActualizarContadorBalas(bool recargaCompleta)
    {
        if (!recargaCompleta)
        {
            Debug.Log("Balas restantes (Ametralladora): " + balasRestantesEnAire);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        puedeDisparar = true;
    }

    private void OnEnable()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded)
        {
            tocandoSueloPrevio = true;
            RecargarBalas();
        }
        else
        {
            tocandoSueloPrevio = false;
            ActualizarContadorBalas(false);
        }
        puedeDisparar = true;
    }
}