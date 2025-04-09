using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float velocidadRotacion = 50f;
    public float intervaloDisparo = 0.5f;

    private bool jugadorEnZona = false;
    private bool controlActivo = false;
    private float tiempoUltimoDisparo;
    private GameObject jugador;

    public float maxRotation = 180;

    void Update()
    {
        if (jugadorEnZona && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            controlActivo = !controlActivo;
            if (controlActivo)
            {
                Debug.Log("Estás en la torreta");
                if (jugador != null)
                {
                    jugador.GetComponent<Jump_Movimiento>().enabled = false; // Desactiva el movimiento del jugador
                }
            }
            else
            {
                if (jugador != null)
                {
                    jugador.GetComponent<Jump_Movimiento>().enabled = true; // Reactiva el movimiento del jugador
                }
            }
        }

        if (controlActivo)
        {
            RotarTorreta();
            Disparar();
        }
    }

    void RotarTorreta()
    {
        float direccion = 0;
        if (Input.GetKey(KeyCode.A))
        {
            direccion = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direccion = -1;
        }
        if (transform.rotation.z >-90 && transform.rotation.z < 90)
        {
            transform.Rotate(Vector3.forward * direccion * velocidadRotacion * Time.deltaTime);

        }
    }

    void Disparar()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > tiempoUltimoDisparo + intervaloDisparo)
        {
            var bala=Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
            bala.GetComponent<Proyectil>().sendDireccion(transform.rotation);
            tiempoUltimoDisparo = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
            jugador = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            controlActivo = false;
            if (jugador != null)
            {
                jugador.GetComponent<Jump_Movimiento>().enabled = true; // Asegura que el jugador pueda moverse al salir
            }
        }
    }
}





/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float velocidadRotacion = 50f;
    public float intervaloDisparo = 0.5f;

    private bool jugadorEnZona = false;
    private bool controlActivo = false;
    private float tiempoUltimoDisparo;

    void Update()
    {
        if (jugadorEnZona && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            controlActivo = !controlActivo;
        }

        if (controlActivo)
        {
            RotarTorreta();
            Disparar();
        }
    }

    void RotarTorreta()
    {
        float direccion = 0;
        if (Input.GetKey(KeyCode.A))
        {
            direccion = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direccion = -1;
        }

        transform.Rotate(Vector3.forward * direccion * velocidadRotacion * Time.deltaTime);
    }

    void Disparar()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > tiempoUltimoDisparo + intervaloDisparo)
        {
            Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);
            tiempoUltimoDisparo = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            controlActivo = false;
        }
    }
}
*/

/*
 * Explicación:
Interacción: Si el jugador está en la zona de la torreta y presiona W o ↑, toma el control de la torreta (o lo suelta si ya lo tenía).

Rotación: A gira la torreta a la izquierda, D a la derecha.

Disparo: La torreta dispara proyectiles hacia abajo (puntoDisparo define el lugar desde donde salen).

Zona de interacción: Usa un Collider2D con la propiedad isTrigger activada para detectar al jugador.

Asegúrate de:

Asignar el prefab del proyectil en proyectilPrefab.

Crear un Transform en la torreta para puntoDisparo.

Etiquetar al jugador como "Player".
*/