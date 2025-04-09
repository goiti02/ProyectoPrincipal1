using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador1_Movimiento : MonoBehaviour
{
    
    Rigidbody2D body;

    float xInput;
    //public float velociry = 1;
    public float gravity = 10f;
    public float velocity = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        xInput = Input.GetAxis("Horizontal"); //Te da 1 si es derecha y -1 izquierda

        body.velocity = new Vector2(xInput * velocity, 0f);

        //=============================================
        //=============================================








        /*
        float yInput = Input.GetAxis("Vertical");
        body.velocity = new Vector2(xInput * velocity, yInput * velocity);
        ===================
        ================
        ==============
        */

        // yInput = Input.GetAxis("Vertical"); //Te da 1 si es derecha y -1 izquierda

        // body.velocity = new Vector2(yInput * velociry, 0f);
        /*
        if (Input.GetKey(KeyCode.RightArrow))
        {
            directiom = 1;
        }
        else { directiom = 0; }*/


    }
}
