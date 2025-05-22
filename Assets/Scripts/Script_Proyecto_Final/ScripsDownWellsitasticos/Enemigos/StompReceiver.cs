// StompReceiver.cs
using UnityEngine;

public class StompReceiver : MonoBehaviour
{
    public EnemyBat parentBat; // Arrastra el objeto MurcielagoEnemigo aqu� en el Inspector

    void Awake()
    {
        if (parentBat == null)
        {
            // Intenta encontrarlo en el padre si no est� asignado
            parentBat = GetComponentInParent<EnemyBat>();
            if (parentBat == null)
            {
                Debug.LogError("StompReceiver no tiene una referencia a su EnemyBat padre. Aseg�rate de asignarlo en el Inspector o que est� en un padre.", this);
                enabled = false;
            }
        }
        // Asegurar que el collider es Trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
        else Debug.LogError("StompReceiver necesita un Collider2D.", this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Condici�n: El jugador est� cayendo sobre el murci�lago.
                // El centro del jugador est� por encima del centro de esta hitbox (aproximaci�n)
                // Y el jugador tiene velocidad vertical negativa.
                bool playerIsAbove = other.transform.position.y > transform.position.y;
                bool playerIsFalling = playerRb.velocity.y < -0.1f; // Un peque�o umbral para asegurar que cae

                if (playerIsFalling && playerIsAbove)
                {
                    Debug.Log("StompReceiver: Jugador detectado pisando.");
                    parentBat.TakeDamageFromStomp();

                    // Opcional: Hacer que el jugador rebote
                    // PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                    // if (playerMovement != null) { playerMovement.ApplyBounce(Vector2.up * bounceForce); }
                }
            }
        }
    }
}