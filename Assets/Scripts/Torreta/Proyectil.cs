using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 5f;
    public LayerMask enemyLayer; // Capa para detectar enemigos

    void Update()
    {
        transform.Translate(Vector3.down * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0) // Verifica si es un enemigo por capa
        {
            Destroy(collision.gameObject); // Destruye al enemigo
            Destroy(gameObject); // Destruye el proyectil
        }
        else if (!collision.CompareTag("Enemy")) // Evita colisionar con el jugador u otros objetos
        {
            Destroy(gameObject);
        }
    }

    public void sendDireccion(Quaternion a)
    {
        transform.rotation = a;
    }
}






/*
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 5f;

   
    void Update()
    {
        transform.Translate(Vector3.down * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) // Evita colisionar con el jugador
        {
            Destroy(gameObject);
        }
    }
    public void sendDireccion(Quaternion a)
    {
        transform.rotation = a;
    }
}

*/