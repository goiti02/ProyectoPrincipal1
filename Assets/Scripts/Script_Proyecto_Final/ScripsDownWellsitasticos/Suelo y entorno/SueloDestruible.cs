using UnityEngine;

public class SueloDestruible : MonoBehaviour
{
    public GameObject efectoDestruccionPrefab; // Opcional: Prefab de part�culas o animaci�n al destruir

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el objeto que colisiona tiene el tag "Bala"
        if (collision.gameObject.CompareTag("Bala"))
        {
            DestruirBloque();

            // Destruimos la bala tambi�n para que no siga atravesando
            // Es buena pr�ctica que la bala se destruya a s� misma al colisionar,
            // pero lo ponemos aqu� por si acaso o como alternativa.
            Destroy(collision.gameObject);
        }
    }

    /* // Alternativa usando Triggers si tus balas son Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bala"))
        {
            DestruirBloque();
            Destroy(other.gameObject); // Destruir la bala
        }
    }
    */

    void DestruirBloque()
    {
        // Opcional: Instanciar efecto de destrucci�n
        if (efectoDestruccionPrefab != null)
        {
            Instantiate(efectoDestruccionPrefab, transform.position, Quaternion.identity);
        }

        // Debug.Log("Bloque destruido!");

        // Destruir este bloque de suelo
        Destroy(gameObject);
    }
}