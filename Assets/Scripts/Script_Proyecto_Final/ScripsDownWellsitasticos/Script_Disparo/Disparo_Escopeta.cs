using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Disparo_Escopeta : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float propulsionForce = 5f;
    public float fireRate = 0.5f; // Disparos por segundo
    private float nextFireTime;
    private Rigidbody2D playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Instanciar el proyectil (si es necesario)
        // GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // ... Lógica específica del proyectil ...

        // Aplicar fuerza de propulsión al jugador en dirección opuesta al disparo
        playerRb.AddForce(-firePoint.up * propulsionForce, ForceMode2D.Impulse);
    }
}