// DamageDealer.cs
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public EnemyBat parentBat; // Arrastra el objeto MurcielagoEnemigo aqu� en el Inspector

    void Awake()
    {
        if (parentBat == null)
        {
            parentBat = GetComponentInParent<EnemyBat>();
            if (parentBat == null)
            {
                Debug.LogError("DamageDealer no tiene una referencia a su EnemyBat padre. Aseg�rate de asignarlo en el Inspector o que est� en un padre.", this);
                enabled = false;
            }
        }
        // Asegurar que el collider es Trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
        else Debug.LogError("DamageDealer necesita un Collider2D.", this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Si el jugador entra en esta hitbox, el murci�lago le hace da�o.
            // La distinci�n "por debajo" vs "por arriba" se maneja teniendo hitboxes separadas.
            Debug.Log("DamageDealer: Jugador detectado en �rea de da�o del murci�lago.");
            parentBat.DealDamageToPlayer(other.gameObject);
        }
    }
}