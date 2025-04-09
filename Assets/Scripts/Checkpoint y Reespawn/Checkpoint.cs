using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color activeColor = Color.green; // Color cuando activas el checkpoint
    private Color inactiveColor = Color.white; // Color inicial

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = inactiveColor; // Se inicia en color blanco
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetCheckpoint(transform.position);
            spriteRenderer.color = activeColor; // Cambia de color al activarse
        }
    }
}
