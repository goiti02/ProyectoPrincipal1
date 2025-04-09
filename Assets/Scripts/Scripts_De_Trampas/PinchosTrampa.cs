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
            Debug.LogWarning("Este objeto de pinchos no tiene el tag 'Pinchos'. �Aseg�rate de asignar el tag correcto!");
        }
    }

    // Opcional: animaci�n o efectos visuales cuando el jugador cae en los pinchos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqu� puedes a�adir efectos de part�culas, sonido, etc.
            Debug.Log("El jugador ha ca�do en los pinchos");
        }
    }
}