using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint2D : MonoBehaviour
{
    // Posición de respawn en 2D
    public Vector2 posicionRespawn;

    private void Start()
    {
        // Si no se ha establecido una posición de respawn personalizada, usar la posición del objeto
        if (posicionRespawn == Vector2.zero)
        {
            posicionRespawn = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Actualizar la posición de respawn del jugador
            other.GetComponent<JugadorRespawn2D>().ActualizarPuntoRespawn(posicionRespawn);
            Debug.Log("Checkpoint actualizado: " + posicionRespawn);
        }
    }

    // Dibujar un gizmo en el editor para ver dónde está el checkpoint
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(posicionRespawn.x, posicionRespawn.y, 0), 0.5f);
    }
}

public class JugadorRespawn2D : MonoBehaviour
{
    private Vector2 puntoRespawn;
    private float zInicial;

    void Start()
    {
        zInicial = transform.position.z;
        puntoRespawn = transform.position; // Guardar la posición inicial como respawn
    }

    public void ActualizarPuntoRespawn(Vector2 nuevaPosicion)
    {
        puntoRespawn = nuevaPosicion;
        Debug.Log("Punto de respawn actualizado: " + puntoRespawn);
    }

    public void Respawn()
    {
        transform.position = new Vector3(puntoRespawn.x, puntoRespawn.y, zInicial);
        Debug.Log("Jugador respawneado en: " + puntoRespawn);
    }
}
