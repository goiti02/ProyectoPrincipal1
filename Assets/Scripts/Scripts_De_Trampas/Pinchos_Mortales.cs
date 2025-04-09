using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos_Mortales : MonoBehaviour

{
    // Posici�n del punto de respawn en 2D
    public Vector2 posicionRespawnCheckpoint;

    private void Start()
    {
        // Si no se ha establecido una posici�n de respawn personalizada, usar la posici�n del objeto
        if (posicionRespawnCheckpoint == Vector2.zero)
        {
            posicionRespawnCheckpoint = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Actualizar la posici�n de respawn del jugador
            other.GetComponent<SistemaRespawnJugador2D>().ActualizarPuntoRespawn(posicionRespawnCheckpoint);
            Debug.Log("Checkpoint actualizado: " + posicionRespawnCheckpoint);
        }
    }

    // Dibujar un gizmo en el editor para ver d�nde est� el checkpoint
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(posicionRespawnCheckpoint.x, posicionRespawnCheckpoint.y, 0), 0.5f);
    }
}

public class SistemaRespawnJugador2D : MonoBehaviour
{
    private Vector2 posicionRespawnActual;
    private Vector2 posicionInicialJuego;
    private float posicionZInicial;

    void Start()
    {
        posicionZInicial = transform.position.z;
        posicionInicialJuego = transform.position; // Guardar la posici�n inicial del juego
        posicionRespawnActual = posicionInicialJuego; // El primer punto de respawn es el inicio
    }

    public void ActualizarPuntoRespawn(Vector2 nuevaPosicionRespawn)
    {
        posicionRespawnActual = nuevaPosicionRespawn;
        Debug.Log("Punto de respawn actualizado: " + posicionRespawnActual);
    }

    public void RespawnJugador()
    {
        transform.position = new Vector3(posicionRespawnActual.x, posicionRespawnActual.y, posicionZInicial);
        Debug.Log("Jugador respawneado en: " + posicionRespawnActual);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pinchos"))
        {
            transform.position = new Vector3(posicionInicialJuego.x, posicionInicialJuego.y, posicionZInicial);
            Debug.Log("Jugador muri� en pinchos. Respawn en posici�n inicial: " + posicionInicialJuego);
        }
    }
}
