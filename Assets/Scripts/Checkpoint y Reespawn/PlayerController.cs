
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Color[] arraycolores;
    private SpriteRenderer playersprite;

    private Vector2 startPosition;
    private int vidas=0;


    private void Start()
    {
        playersprite = GetComponent<SpriteRenderer>();
        vidas = arraycolores.Length;
        handleVidas();

        startPosition = transform.position;
    }

    public void Respawn()
    {
        Vector2 checkpointPosition = GameManager.instance.GetCheckpoint();
        transform.position = checkpointPosition != Vector2.zero ? checkpointPosition : startPosition;
        if(vidas>0) vidas--;
        handleVidas();

    }


    private void handleVidas()
    {
        if (vidas <= 0)
        {
            Death();
        }
        else
        {
            playersprite.color = arraycolores[(vidas-1)];
        }

    }

    private void Death()
    {
        SceneManager.LoadScene("LV");
    }


}
