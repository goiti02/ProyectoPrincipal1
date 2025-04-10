using UnityEngine;

public class Player_Disparos_Down : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootForce = 10f;
    public float shootCooldown = 0.2f;

    private float lastShootTime;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > lastShootTime + shootCooldown)
        {
            ShootDown();
            lastShootTime = Time.time;
        }
    }

    void ShootDown()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * shootForce;
        }
    }
}
