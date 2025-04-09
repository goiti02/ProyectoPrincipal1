using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Movimiento : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float jumpForce = 10f; // Fuerza del salto
    public float buttonTime = 0.3f; // Tiempo máximo de salto sostenido
    public float coyoteTime = 0.2f; // Tiempo de gracia para saltar después de salir del suelo

    private float jumpTime;
    private float coyoteCounter;
    private bool jumping;
    private bool hasJumped;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Ground Check")]
    public Transform groundCheck; // Objeto vacío en los pies del personaje
    public LayerMask groundLayer; // Layer del suelo
    private bool isGrounded;
    public float groundcheckerdistance = 0.7f;

    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Invertir el sprite según la dirección del movimiento
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }

        // Detectar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Resetear el contador de coyote si está en el suelo
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            hasJumped = false; // Permite saltar nuevamente al tocar el suelo
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Inicio del salto (solo si no ha saltado aún en el aire)
        if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0f && !hasJumped)
        {
            jumping = true;
            jumpTime = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplica el salto
            hasJumped = true; // Evita dobles saltos en el aire
        }

        // Mantener el salto si se mantiene presionado el botón
        if (jumping)
        {
            jumpTime += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonTime)
            {
                jumping = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Corta el salto
            }
        }
    }
}






/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Movimiento : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float jumpForce = 10f; // Fuerza del salto
    public float buttonTime = 0.3f; // Tiempo máximo de salto sostenido
    public float coyoteTime = 0.2f; // Tiempo de gracia para saltar después de salir del suelo

    private float jumpTime;
    private float coyoteCounter;
    private bool jumping;
    private bool hasJumped;

    private Rigidbody2D rb;

    [Header("Ground Check")]
    public Transform groundCheck; // Objeto vacío en los pies del personaje
    public LayerMask groundLayer; // Layer del suelo
    private bool isGrounded;
    public float groundcheckerdistance = 0.7f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        void OnDrawGizmos()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundcheckerdistance);
            }
        }
    }

    void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Detectar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Resetear el contador de coyote si está en el suelo
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            hasJumped = false; // Permite saltar nuevamente al tocar el suelo
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Inicio del salto (solo si no ha saltado aún en el aire)
        if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0f && !hasJumped)
        {
            jumping = true;
            jumpTime = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplica el salto
            hasJumped = true; // Evita dobles saltos en el aire
        }

        // Mantener el salto si se mantiene presionado el botón
        if (jumping)
        {
            jumpTime += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonTime)
            {
                jumping = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Corta el salto
            }
        }
    }
}

*/