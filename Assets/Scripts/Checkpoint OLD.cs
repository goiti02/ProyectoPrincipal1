/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint  : MonoBehaviour
{
    // Esta posición se utilizará para el respawn (puede ser diferente a la posición del trigger)
    public Vector3 posicionRespawn;

    // Variable para visualizar en el editor
    public bool mostrarEnEditor = true;

    // Variable para mantener la coordenada Z constante en juego 2D
    [Tooltip("Valor de Z que se mantendrá fijo para el jugador al hacer respawn (para juego 2D)")]
    public float zFija = 0f;

    private void Start()
    {
        // Si no se ha establecido una posición de respawn personalizada, usar la posición del objeto
        if (posicionRespawn == Vector3.zero)
        {
            posicionRespawn = transform.position;
        }

        // Asegurar que la posición de respawn respeta la Z fija para juego 2D
        posicionRespawn = new Vector3(posicionRespawn.x, posicionRespawn.y, zFija);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si es el jugador quien ha entrado en el checkpoint
        if (other.CompareTag("Player"))
        {
            // Obtener referencia al componente del jugador
            Jugador_Reespawn_Cambio_Color jugadorScript = other.GetComponent<Jugador_Reespawn_Cambio_Color>();

            if (jugadorScript != null)
            {
                // Asegurar que la Z está fija antes de actualizar el punto de respawn
                Vector3 respawnCorregido = new Vector3(posicionRespawn.x, posicionRespawn.y, zFija);

                // Actualizar el punto de respawn en el script del jugador
                jugadorScript.ActualizarPuntoRespawn(respawnCorregido);

                Debug.Log("Checkpoint activado. Punto de respawn establecido en: " + respawnCorregido);
            }
        }
    }

    // Dibujar un gizmo en el editor para ver donde está el checkpoint
    private void OnDrawGizmos()
    {
        if (!mostrarEnEditor) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);

        // Dibujar también la posición de respawn si es diferente
        if (posicionRespawn != transform.position && posicionRespawn != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(posicionRespawn, 0.3f);
            Gizmos.DrawLine(transform.position, posicionRespawn);
        }

        // Opcional: Dibujar un pequeño indicador que muestra que este es un checkpoint 2D
        Gizmos.color = Color.yellow;
        Vector3 pos2D = transform.position + Vector3.up * 0.7f;
        Gizmos.DrawLine(pos2D, pos2D + Vector3.right * 0.3f);
        Gizmos.DrawLine(pos2D, pos2D - Vector3.right * 0.3f);
        Gizmos.DrawLine(pos2D, pos2D + Vector3.up * 0.3f);
    }
}



/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Esta posición se utilizará para el respawn (puede ser diferente a la posición del trigger)
    public Vector3 posicionRespawn;

    // Variable para visualizar en el editor
    public bool mostrarEnEditor = true;

    // Variable para mantener la coordenada Z constante en juego 2D
    [Tooltip("Valor de Z que se mantendrá fijo para el jugador al hacer respawn (para juego 2D)")]
    public float zFija = 0f;

    // Variable para referenciar al jugador si es necesario
    private Jugador_Reespawn_Cambio_Color jugador;

    private void Start()
    {
        // Si no se ha establecido una posición de respawn personalizada, usar la posición del objeto
        if (posicionRespawn == Vector3.zero)
        {
            posicionRespawn = transform.position;
        }

        // Asegurar que la posición de respawn respeta la Z fija para juego 2D
        posicionRespawn = new Vector3(posicionRespawn.x, posicionRespawn.y, zFija);

        // Intentar obtener referencia al jugador para usar su valor Z preferido
        jugador = FindObjectOfType<Jugador_Reespawn_Cambio_Color>();
        if (jugador != null)
        {
            // Usar el valor Z del jugador para mantener coherencia
            zFija = jugador.zPosition;
            posicionRespawn = new Vector3(posicionRespawn.x, posicionRespawn.y, zFija);

            Debug.Log("Checkpoint: Usando zPosition del jugador: " + zFija);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si es el jugador quien ha entrado en el checkpoint
        if (other.CompareTag("Player"))
        {
            // Obtener referencia al componente del jugador
            Jugador_Reespawn_Cambio_Color jugadorScript = other.GetComponent<Jugador_Reespawn_Cambio_Color>();

            if (jugadorScript != null)
            {
                // Asegurar que la Z está fija antes de actualizar el punto de respawn
                Vector3 respawnCorregido = new Vector3(posicionRespawn.x, posicionRespawn.y, jugadorScript.zPosition);

                // Actualizar el punto de respawn en el script del jugador
                jugadorScript.ActualizarPuntoRespawn(respawnCorregido);

                Debug.Log("Checkpoint activado. Punto de respawn establecido en: " + respawnCorregido);
            }
        }
    }

    // Dibujar un gizmo en el editor para ver donde está el checkpoint
    private void OnDrawGizmos()
    {
        if (!mostrarEnEditor) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);

        // Dibujar también la posición de respawn si es diferente
        if (posicionRespawn != transform.position && posicionRespawn != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(posicionRespawn, 0.3f);
            Gizmos.DrawLine(transform.position, posicionRespawn);
        }

        // Opcional: Dibujar un pequeño indicador que muestra que este es un checkpoint 2D
        Gizmos.color = Color.yellow;
        Vector3 pos2D = transform.position + Vector3.up * 0.7f;
        Gizmos.DrawLine(pos2D, pos2D + Vector3.right * 0.3f);
        Gizmos.DrawLine(pos2D, pos2D - Vector3.right * 0.3f);
        Gizmos.DrawLine(pos2D, pos2D + Vector3.up * 0.3f);
    }
}


/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Esta posición se utilizará para el respawn (puede ser diferente a la posición del trigger)
    public Vector3 posicionRespawn;

    // Variable para visualizar en el editor
    public bool mostrarEnEditor = true;

    private void Start()
    {
        // Si no se ha establecido una posición de respawn personalizada, usar la posición del objeto
        if (posicionRespawn == Vector3.zero)
        {
            posicionRespawn = transform.position;
        }
    }

    // Dibujar un gizmo en el editor para ver donde está el checkpoint
    private void OnDrawGizmos()
    {
        if (!mostrarEnEditor) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);

        // Dibujar también la posición de respawn si es diferente
        if (posicionRespawn != transform.position && posicionRespawn != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(posicionRespawn, 0.3f);
            Gizmos.DrawLine(transform.position, posicionRespawn);
        }
    }
}

*/