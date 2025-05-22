using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Disparo_Basico : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float playerThrustForce = 5f;
    public float tiempoDeBala = 2f; // Tiempo en segundos antes de que la bala desaparezca
    public int maxBalas = 8; // Cantidad máxima de balas
    private int balasRestantes;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private Rigidbody2D playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogError("El script Disparo_Avanzado debe estar adjunto a un GameObject con un Rigidbody2D.");
            enabled = false;
        }
        balasRestantes = maxBalas;
        ActualizarContadorBalas();
    }

    void Update()
    {
        // Chequear si está en el suelo para recargar las balas
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded)
        {
            RecargarBalas();
        }

        // Intentar disparar
        if (Input.GetButtonDown("Fire1") && balasRestantes > 0)
        {
            DispararAbajo();
        }
        else if (Input.GetButtonDown("Fire1") && balasRestantes <= 0)
        {
            Debug.Log("¡No quedan balas!");
        }
    }

    void DispararAbajo()
    {
        // Instanciar la bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

        // Disparar hacia abajo
        rbBullet.velocity = -firePoint.up * bulletSpeed;

        // Empujar al jugador hacia arriba
        if (playerRb != null)
        {
            playerRb.AddForce(firePoint.up * playerThrustForce, ForceMode2D.Impulse);
        }

        // Decrementar el contador de balas y actualizar el Debug.Log
        balasRestantes--;
        ActualizarContadorBalas();

        // Destruir la bala después de un tiempo
        Destroy(bullet, tiempoDeBala);
    }

    void RecargarBalas()
    {
        if (balasRestantes < maxBalas)
        {
            balasRestantes = maxBalas;
            ActualizarContadorBalas();
        }
    }

    void ActualizarContadorBalas()
    {
        Debug.Log("Balas restantes: " + balasRestantes);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green; // Cambié el color para distinguirlo
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}





/*
 * using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Disparo_Basico : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float playerThrustForce = 5f; // Nueva variable para el empuje hacia arriba del jugador

    private Rigidbody2D playerRb; // Referencia al Rigidbody2D del jugador

    void Start()
    {
        // Obtener el componente Rigidbody2D del GameObject al que está adjunto este script
        playerRb = GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogError("El script Disparo_Basico debe estar adjunto a un GameObject con un Rigidbody2D.");
            enabled = false; // Deshabilitar el script si no hay Rigidbody2D
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Botón izquierdo del ratón por defecto
        {
            ShootDown();
        }
    }

    void ShootDown()
    {
        // Instanciar la bala en la posición y rotación del firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

        // Disparar la bala hacia ABAJO (cambiando firePoint.up a -firePoint.up)
        rbBullet.velocity = -firePoint.up * bulletSpeed;

        // Aplicar una fuerza de empuje hacia ARRIBA al jugador
        if (playerRb != null)
        {
            playerRb.AddForce(firePoint.up * playerThrustForce, ForceMode2D.Impulse);
        }
    }
}

*/

/*
using UnityEngine;

public class Disparo_Basico : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Botón izquierdo del ratón por defecto
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed; // Asumiendo que firePoint apunta hacia la dirección de disparo
    }
}

*/