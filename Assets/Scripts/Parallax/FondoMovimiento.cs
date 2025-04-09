using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    [SerializeField] private Vector2 velocidad; // control de la velocidad de los fondos
 private Vector2 offset; //Almacena el desplazamiento aplicado a la textura.
 private Material material; //Guarda la referencia al material del SpriteRenderer del fondo.
 [SerializeField]
 private Rigidbody2D playerRb; //Referencia al Rigidbody2D del jugador, usado para obtener su velocidad y aplicarla al fondo.

 private void Awake()
 {
     material = GetComponent<SpriteRenderer>().material;
     /* Obtiene el material del SpriteRenderer en el GameObject del fondo.
      * 
        Esto permite modificar su propiedad mainTextureOffset, que es la clave para el efecto de desplazamiento.
      */

 }

 private void Update()
 {
     if (playerRb != null)
     {
            //offset = (playerRb.linearVelocity.x * 0.1f) * velocidad * Time.deltaTime;
            offset = (playerRb.velocity.y * 0.1f) * velocidad * Time.deltaTime;
         material.mainTextureOffset += offset;
     }
 }

    /*
    [SerializeField] private Vector2 velocidadMovimiento;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugadorRB;
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        jugadorRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        offset = (jugadorRB.velocity.x +0.1f)* velocidadMovimiento * Time.deltaTime;
        material.mainTextureOffset += offset;
    }*/

}
