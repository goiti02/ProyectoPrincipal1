using UnityEngine;
using System.Collections; // Necesario para Coroutines

[RequireComponent(typeof(Rigidbody2D))]
public class Disparo_Ametralladora : MonoBehaviour
{
    [Header("Referencias")]
    public Transform firePoint;          // Punto de origen del disparo
    public GameObject bulletPrefab;      // Prefab de la bala
    public Transform groundCheck;        // Objeto para detectar el suelo

    [Header("Parámetros del Arma")]
    public float velocidadDeDisparo = 0.5f; // Tiempo mínimo ENTRE ráfagas (segundos)
    public float tiempoDeVidaBala = 1.5f;  // Tiempo en segundos antes de que la bala desaparezca
    // public float dañoDisparo = 1f; // El daño debería estar en la bala (Script Bala.cs) o pasarse al instanciar
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
            enabled = false; // Desactiva el script si no hay Rigidbody2D
            return;
        }

        if (firePoint == null || bulletPrefab == null || groundCheck == null)
        {
            Debug.LogError("¡Asigna Fire Point, Bullet Prefab y Ground Check en el Inspector!", this);
            enabled = false;
            return;
        }

        // Inicializamos con la munición llena (se recarga al tocar suelo)
        balasRestantesEnAire = disparosMaximosAire;
        ActualizarContadorBalas(true); // true indica que es estado inicial o recarga
    }

    void Update()
    {
        // Chequear si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Lógica de Recarga al tocar el suelo
        if (isGrounded)
        {
            if (!tocandoSueloPrevio) // Solo recarga y loguea la primera vez que toca el suelo
            {
                Debug.Log("¡Tocado el suelo!");
                if (balasRestantesEnAire < disparosMaximosAire)
                {
                    RecargarBalas();
                }
                tocandoSueloPrevio = true;
                // Permitimos disparar inmediatamente al tocar suelo si no lo hacía ya
                if (!puedeDisparar)
                {
                    // Reiniciamos el cooldown para poder disparar desde el suelo
                    // puedeDisparar = true; // Opcional: decidir si quieres cooldown en suelo
                }
            }
        }
        else // Está en el aire
        {
            tocandoSueloPrevio = false; // Marcamos que ya no está tocando el suelo
        }


        // Intentar disparar una ráfaga (solo si se presiona el botón, hay balas y no está en cooldown)
        // Usamos GetButtonDown para que cada pulsación lance UNA ráfaga completa.
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
        puedeDisparar = false; // Previene iniciar otra ráfaga hasta que esta termine y pase el cooldown

        // Debug.Log("Iniciando ráfaga...");

        for (int i = 0; i < proyectilesPorRafaga; i++)
        {
            // Comprobamos si aún quedan balas ANTES de disparar la siguiente de la ráfaga
            if (balasRestantesEnAire <= 0)
            {
                Debug.Log("¡Ametralladora sin balas en el aire durante ráfaga!");
                break; // Salir del bucle si nos quedamos sin balas a mitad de ráfaga
            }

            // --- Disparar una bala ---
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

            if (rbBullet != null)
            {
                // Disparar hacia abajo (o en la dirección de firePoint.up si lo rotas)
                rbBullet.velocity = -firePoint.up * velocidadBala;
            }
            else
            {
                Debug.LogError("El prefab de la bala no tiene Rigidbody2D!", bulletPrefab);
            }

            // Añadir el script Bala si no lo tiene (importante para la destrucción)
            if (bullet.GetComponent<Bala>() == null)
            {
                bullet.AddComponent<Bala>();
                Debug.LogWarning("Se añadió script 'Bala' dinámicamente al prefab. Es mejor añadirlo directamente al prefab.", bullet);
            }

            // Empujar al jugador hacia arriba solo si está en el aire
            if (!isGrounded && playerRb != null)
            {
                // Aplicamos fuerza hacia arriba. Usamos Impulse para un empujón instantáneo.
                playerRb.AddForce(firePoint.up * fuerzaEmpujeDisparo, ForceMode2D.Impulse);
                // Opcional: Limitar velocidad vertical para evitar exceso de impulso
                // playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -Mathf.Infinity, maxVerticalVelocityAfterShot));
            }

            // Decrementar el contador de balas y actualizar el Debug.Log
            balasRestantesEnAire--;
            ActualizarContadorBalas(false);

            // Destruir la bala después de un tiempo
            Destroy(bullet, tiempoDeVidaBala);
            // --- Fin Disparar una bala ---


            // Esperar el tiempo entre balas DENTRO de la ráfaga
            if (i < proyectilesPorRafaga - 1) // No esperar después de la última bala de la ráfaga
            {
                yield return new WaitForSeconds(retrasoEntreBalasRafaga);
            }
        }

        // Esperar el cooldown ENTRE ráfagas después de que la ráfaga completa haya terminado
        yield return new WaitForSeconds(velocidadDeDisparo);
        puedeDisparar = true; // Permitir disparar de nuevo
        // Debug.Log("Ametralladora lista para disparar de nuevo.");
    }

    void RecargarBalas()
    {
        if (balasRestantesEnAire < disparosMaximosAire)
        {
            balasRestantesEnAire = disparosMaximosAire;
            Debug.Log("¡Ametralladora Recargada!");
            ActualizarContadorBalas(true); // true indica recarga completa
        }
    }

    void ActualizarContadorBalas(bool recargaCompleta)
    {
        if (!recargaCompleta) // Solo loguea el conteo si no es una recarga
        {
            Debug.Log("Balas restantes (Ametralladora): " + balasRestantesEnAire);
        }
    }

    // Dibuja el gizmo para el chequeo de suelo en el editor
    private void OnDrawGizmosSelected() // Cambiado a Selected para no saturar la vista
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow; // Color diferente para distinguir
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    // Asegúrate de que si este script se desactiva (por cambio de arma),
    // la corrutina de disparo se detenga.
    private void OnDisable()
    {
        StopAllCoroutines(); // Detiene la ráfaga si se cambia de arma a mitad
        puedeDisparar = true; // Resetea el estado por si se vuelve a activar
    }

    // Si se activa (cambio de arma), recarga si está en el suelo.
    private void OnEnable()
    {
        // Forzamos un chequeo de suelo al activar por si empezamos en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded)
        {
            tocandoSueloPrevio = true; // Asume que empieza tocando suelo si está grounded
            RecargarBalas();
        }
        else
        {
            // Si se activa en el aire, podría mantener las balas que tenía
            // O podrías decidir resetearlas aquí también. Por ahora las mantiene.
            tocandoSueloPrevio = false;
            // balasRestantesEnAire = disparosMaximosAire; // Descomentar para recargar siempre al activar
            ActualizarContadorBalas(false);
        }
        puedeDisparar = true; // Asegura que puede disparar al ser activada
    }
}