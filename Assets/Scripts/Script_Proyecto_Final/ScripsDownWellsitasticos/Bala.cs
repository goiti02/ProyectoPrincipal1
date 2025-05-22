using UnityEngine;

public class Bala : MonoBehaviour
{
    // Puedes añadir variables como daño aquí si quieres
    // public float damage = 1f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con suelo destruible O con un enemigo (u otro objeto sólido)
        if (collision.gameObject.CompareTag("SueloDestruible") ||
            collision.gameObject.CompareTag("Enemigo") || // Asegúrate de tener este tag si tienes enemigos
            collision.gameObject.CompareTag("Suelo")) // Tag para suelo NO destruible
        {
            // Podrías añadir lógica aquí antes de destruir, como aplicar daño al enemigo
            // if (collision.gameObject.CompareTag("Enemigo")) {
            //     collision.gameObject.GetComponent<VidaEnemigo>()?.RecibirDaño(damage);
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