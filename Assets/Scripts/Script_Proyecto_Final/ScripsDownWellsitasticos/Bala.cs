using UnityEngine;

public class Bala : MonoBehaviour
{
    // Puedes a�adir variables como da�o aqu� si quieres
    // public float damage = 1f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con suelo destruible O con un enemigo (u otro objeto s�lido)
        if (collision.gameObject.CompareTag("SueloDestruible") ||
            collision.gameObject.CompareTag("Enemigo") || // Aseg�rate de tener este tag si tienes enemigos
            collision.gameObject.CompareTag("Suelo")) // Tag para suelo NO destruible
        {
            // Podr�as a�adir l�gica aqu� antes de destruir, como aplicar da�o al enemigo
            // if (collision.gameObject.CompareTag("Enemigo")) {
            //     collision.gameObject.GetComponent<VidaEnemigo>()?.RecibirDa�o(damage);
            // }

            Destroy(gameObject); // Destruye la bala
        }
    }

    /* // Alternativa si usas Triggers
   void OnTriggerEnter2D(Collider2D other)
   {
       if (other.CompareTag("SueloDestruible") || other.CompareTag("Enemigo") || other.CompareTag("Suelo"))
       {
           Destroy(gameObject);
       }
   }
   */
}