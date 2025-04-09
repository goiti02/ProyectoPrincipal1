using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JugadorMovimiento2D : MonoBehaviour
{
    [Header("Movimiento del jugador")]
    public float moveSpeed = 3f;
    private float zInicial;
    private Rigidbody2D rb;

    [Header("Vida")]
    public float vida_Maxima_Jugador = 100f;
    public float vida_Jugador = 100f;
    public TextMeshProUGUI Vida;

    void Start()
    {
        zInicial = transform.position.z; // Bloquear el eje Z
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1; // Asegurar que tiene gravedad normal si es necesario
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Bloquear la rotación completamente
        }
        PrintVida();
    }

    void Update()
    {
        PlayerInputs();
        PrintVida();
        MantenerPosicionZ();
    }

    void PlayerInputs()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Ajustar escala para voltear al personaje sin rotarlo
        if (moveHorizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        }
        else if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
        }
    }

    void MantenerPosicionZ()
    {
        if (transform.position.z != zInicial)
        {
            Vector3 posicionCorregida = transform.position;
            posicionCorregida.z = zInicial;
            transform.position = posicionCorregida;
        }
    }

    public void PrintVida()
    {
        Vida.text = vida_Jugador.ToString();
    }
}
