using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Asigna en el inspector la imagen de pausa
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Muestra la imagen de pausa
        Time.timeScale = 0f; // Detiene el tiempo del juego
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Oculta la imagen de pausa
        Time.timeScale = 1f; // Reactiva el tiempo del juego
        isPaused = false;
    }
}
