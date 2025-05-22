using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movimiento_PF : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minJumpForce = 5f;
    public float maxJumpForce = 12f;
    public float jumpChargeSpeed = 20f; // Velocidad a la que se carga la fuerza del salto
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    public bool isGrounded;
    private float currentJumpForce;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentJumpForce = minJumpForce;
    }

    void Update()
    {
        // Movimiento horizontal
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Chequear si está en el suelo (esta línea ya la tenías)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Salto con la tecla "W" (nueva lógica)
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, maxJumpForce), ForceMode2D.Impulse); // Usando maxJumpForce para un salto directo con "W"
            isJumping = false; // Reiniciar la bandera de salto si estás usando la lógica de carga con otra tecla
        }

        // Inicio del salto (con la tecla de salto original, por ejemplo, Espacio)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            currentJumpForce = minJumpForce; // Reiniciar la fuerza de salto
        }

        // Carga de la fuerza del salto mientras se mantiene presionado (con la tecla de salto original)
        if (isJumping && Input.GetButton("Jump"))
        {
            currentJumpForce = Mathf.Clamp(currentJumpForce + jumpChargeSpeed * Time.deltaTime, minJumpForce, maxJumpForce);
        }

        // Ejecución del salto al soltar el botón (con la tecla de salto original)
        if (isJumping && Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
            isJumping = false;
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




/*
 * 
 * 
 * 
 * 
 * 
 * 
 * 
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movimiento_PF : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minJumpForce = 5f;
    public float maxJumpForce = 12f;
    public float jumpChargeSpeed = 20f; // Velocidad a la que se carga la fuerza del salto
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    public bool isGrounded;
    private float currentJumpForce;
    private bool isJumping = false;
    public float radius;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentJumpForce = minJumpForce;
    }

    void Update()
    {
        // Movimiento horizontal
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Chequear si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Inicio del salto
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            currentJumpForce = minJumpForce; // Reiniciar la fuerza de salto
        }

        // Carga de la fuerza del salto mientras se mantiene presionado
        if (isJumping && Input.GetButton("Jump"))
        {
            currentJumpForce = Mathf.Clamp(currentJumpForce + jumpChargeSpeed * Time.deltaTime, minJumpForce, maxJumpForce);
        }

        // Ejecución del salto al soltar el botón
        /*if (isJumping && Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
            isJumping = false;
        } 
    }
}

*/