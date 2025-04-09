using UnityEngine;
using UnityEngine.UI;

public class GameEndTrigger : MonoBehaviour
{
    public GameObject gameOverCanvas; // Asigna el Canvas en el Inspector

    private void Start()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false); // Asegura que el Canvas esté oculto al inicio
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si el jugador entra en la zona
        {
            if (gameOverCanvas != null)
            {
                gameOverCanvas.SetActive(true); // Activa el Canvas de "Fin del Juego"
                Time.timeScale = 0f; // Pausa el juego
            }
        }
    }
}

