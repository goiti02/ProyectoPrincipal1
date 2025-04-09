using UnityEngine;

public class Bullet2D : MonoBehaviour
{
    public float speed = 10f; // Velocidad de la bala
    public float lifetime = 3f; // Tiempo antes de autodestruirse
    public LayerMask enemyLayer; // Para detectar solo enemigos

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destruir la bala después de un tiempo
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // Movimiento horizontal
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0) // Verifica si es un enemigo
        {
            Destroy(other.gameObject); // Destruir enemigo
            Destroy(gameObject); // Destruir bala
        }
    }
}
