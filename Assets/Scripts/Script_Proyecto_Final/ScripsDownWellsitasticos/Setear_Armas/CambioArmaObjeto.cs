using UnityEngine;

public class CambioArmaObjeto : MonoBehaviour
{
    // �IMPORTANTE! Escribe aqu� el nombre EXACTO del script del arma que este objeto otorga
    // Ejemplo: "Disparo_Escopeta", "Ametralladora"
    public string nombreScriptArmaNueva;

    // Lista de TODOS los posibles scripts de armas que el jugador puede tener.
    // Debes a�adir aqu� los nombres de todos tus scripts de disparo.
    private readonly string[] todosLosScriptsDeArmas = {
        "Disparo_Basico",
        "Disparo_Escopeta",
        "Ametralladora"
        // A�ade aqu� los nombres de otros scripts de armas que crees
    };

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que entra en el trigger es el jugador (por tag)
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Jugador ha recogido objeto para cambiar a arma: {nombreScriptArmaNueva}");

            GameObject jugador = other.gameObject;

            // Desactivar todos los scripts de armas conocidos
            foreach (string nombreScript in todosLosScriptsDeArmas)
            {
                // Intentamos obtener el componente por su nombre
                MonoBehaviour scriptArma = jugador.GetComponent(nombreScript) as MonoBehaviour;
                if (scriptArma != null)
                {
                    scriptArma.enabled = false;
                    // Debug.Log($"Script desactivado: {nombreScript}");
                }
            }

            // Activar el nuevo script de arma
            MonoBehaviour nuevoScript = jugador.GetComponent(nombreScriptArmaNueva) as MonoBehaviour;
            if (nuevoScript != null)
            {
                nuevoScript.enabled = true;
                Debug.Log($"Script activado: {nombreScriptArmaNueva}");

                // Opcional: Si tus scripts de arma necesitan reiniciarse (ej: recargar munici�n al cambiar)
                // podr�as llamar a un m�todo p�blico espec�fico aqu�, por ejemplo:
                // if (nuevoScript is ArmaBase arma) // Suponiendo que tienes una clase base
                // {
                //     arma.OnEquip();
                // }
            }
            else
            {
                Debug.LogError($"Error: No se encontr� el script '{nombreScriptArmaNueva}' en el jugador. Aseg�rate de que el jugador tenga este componente adjunto y el nombre sea correcto.");
            }

            // Destruir el objeto de power-up
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Peque�a validaci�n al inicio
        if (string.IsNullOrEmpty(nombreScriptArmaNueva))
        {
            Debug.LogError("�Error! El objeto de cambio de arma no tiene asignado un 'nombreScriptArmaNueva' en el inspector.", this);
        }
    }
}