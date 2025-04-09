using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchosTrampa : MonoBehaviour
{
    // Este script es simple, solo necesita tener el tag "Pinchos" en el objeto
    // para que el jugador lo detecte correctamente

    private void Start()
    {
        // Verificar que el objeto tiene el tag correcto
        if (!CompareTag("Pinchos"))
        {
            Debug.LogWarning("Este objeto de pinchos no tiene el tag 'Pinchos'. ¡Asegúrate de asignar el tag correcto!");
        }
    }

    // Opcional: animación o efectos visuales cuando el jugador cae en los pinchos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí puedes añadir efectos de partículas, sonido, etc.
            Debug.Log("El jugador ha caído en los pinchos");
        }
    }
}