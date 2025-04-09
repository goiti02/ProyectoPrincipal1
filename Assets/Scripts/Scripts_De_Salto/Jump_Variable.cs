using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Variable : MonoBehaviour
{
    public float jumpForce = 10f; // Fuerza máxima del salto
    public float buttonTime = 0.3f; // Tiempo máximo de salto
    public float coyoteTime = 0.1f; // Tiempo extra para saltar después de dejar el suelo
    private float jumpTime;
    private float coyoteCounter;
    private bool jumping;
    private bool isGrounded;
    private bool hasJumped; // Evita múltiples saltos en el aire
    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundcheckerdistance = 0.7f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundcheckerdistance);
        }
    }

    void Update()
    {
        // Verifica si el personaje está tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundcheckerdistance, groundLayer);

        // Si está en el suelo, reiniciamos el Coyote Time y permitimos saltar de nuevo
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            hasJumped = false; // Reseteamos para permitir otro salto
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Inicio del salto (solo si no ha saltado aún en el aire)
        if (Input.GetKeyDown(KeyCode.Space) && (coyoteCounter > 0f && isGrounded) && !hasJumped)
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
 * 
 * VERSION 1 DE COLLOTE TIME
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Variable : MonoBehaviour
{
    public float jumpForce = 10f; // Fuerza máxima del salto
    public float buttonTime = 0.3f; // Tiempo máximo de salto
    public float coyoteTime = 0.1f; // Tiempo extra para saltar después de salir del suelo
    private float jumpTime;
    private float coyoteCounter;
    private bool jumping;
    private bool isGrounded;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D
    }

    void Update()
    {
        // Verifica si el personaje está tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Si está en el suelo, reiniciamos el Coyote Time
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Inicio del salto (permite el salto incluso dentro del coyote time)
        if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0f)
        {
            jumping = true;
            jumpTime = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Salto inicial
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


/*

//SALTO NORMAL VARIABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Variable : MonoBehaviour
{
    public float jumpForce = 10f; // Fuerza máxima del salto
    public float buttonTime = 0.3f; // Tiempo máximo de salto
    private float jumpTime;
    private bool jumping;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Approximately(rb.velocity.y, 0))
        {
            jumping = true;
            jumpTime = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Salto inicial
        }

        if (jumping)
        {
            jumpTime += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonTime)
            {
                jumping = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Reduce la velocidad al soltar
            }
        }
    }
}

*/


//========================================================
//========================================================
//========================================================
//========================================================
//========================================================

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Variable : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del salto
    public float buttonTime = 0.3f;
    private float jumpTime;
    private bool jumping;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Approximately(rb.velocity.y, 0))
        {
            jumping = true;
            jumpTime = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplica el salto
        }

        if (jumping)
        {
            jumpTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonTime)
        {
            jumping = false;
        }
    }
}
*/

//  ===============================
// ================================
// ================================

/*
 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Variable : MonoBehaviour
{

    public float jumpForce = 5f; // Fuerza del salto
    public float buttonTime = 0.3f;
    float jumpTime;
    bool jumping;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping = true;
                jumpTime = 0;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Aplica el salto
            }

            if (jumping)
            {
                jumpTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space) | jumpTime > buttonTime)
            {
                jumping = false;
            }
        }
    }
}

 
*/