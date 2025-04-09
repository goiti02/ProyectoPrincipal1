using UnityEngine;

public class GameManager : MonoBehaviour
//guardar y recordar la posición del último punto de control que el jugador ha alcanzado en el juego.
{
    public static GameManager instance;
    private Vector2 lastCheckpoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

    public Vector2 GetCheckpoint()
    {
        return lastCheckpoint;
    }
}
