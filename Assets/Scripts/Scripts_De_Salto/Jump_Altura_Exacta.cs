using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Altura_Exacta : MonoBehaviour
{
    public float jumpHeight = 5f; // Ajusta este valor según la altura deseada
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del objeto
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Detecta si se presiona la barra espaciadora
        {
            Jump();
        }
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        rb.velocity = new Vector2(rb.velocity.x, 0); // Resetea la velocidad vertical antes de saltar
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}

