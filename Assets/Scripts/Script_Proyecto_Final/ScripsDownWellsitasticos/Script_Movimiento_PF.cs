using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Movimiento_PF : MonoBehaviour
{
    Rigidbody2D body;
    float xInput;
    public float velocity = 5f;
    public float jumpForce = 10f; // Fuerza del salto
    public Transform groundCheck; // Punto para verificar el suelo
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround; // Capas que se consideran suelo
    private bool isGrounded;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(xInput * velocity, body.velocity.y); // Mantener la velocidad vertical existente

        // Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Lógica de salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}