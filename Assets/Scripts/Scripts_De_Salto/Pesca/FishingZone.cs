using System.Collections;
using UnityEngine;

public class FishingZone : MonoBehaviour
{
    private bool isFishing = false;
    private bool canCatchFish = false;
    private float fishingDelay;
    private bool playerInside = false;
    private Jump_Movimiento playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerMovement = other.GetComponent<Jump_Movimiento>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            isFishing = false;
            canCatchFish = false;

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (playerInside && !isFishing && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (playerMovement != null)
            {
                playerMovement.enabled = false; // Desactiva el movimiento al empezar a pescar
            }
            StartFishing();
        }

        if (canCatchFish && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("¡Has pescado un pez!");
            canCatchFish = false;
            EndFishing();
        }
    }

    private void StartFishing()
    {
        isFishing = true;
        fishingDelay = Random.Range(2f, 20f);
        Debug.Log("Esperando a que pique un pez...");
        StartCoroutine(FishingCoroutine());
    }

    private IEnumerator FishingCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fishingDelay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("¡Demasiado pronto! El pez se escapó...");
                EndFishing();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (playerInside)
        {
            Debug.Log("¡Está picando! Pulsa 'ESPACIO' en 0.7 segundos.");
            canCatchFish = true;

            yield return new WaitForSeconds(0.7f);

            if (canCatchFish)
            {
                Debug.Log("El pez se escapó...");
                EndFishing();
            }
        }
    }

    private void EndFishing()
    {
        isFishing = false;
        canCatchFish = false;

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}


/*

    VERSION 1.0

using System.Collections;
using UnityEngine;

public class FishingZone : MonoBehaviour
{
    private bool isFishing = false;
    private bool canCatchFish = false;
    private float fishingDelay;
    private bool playerInside = false;
    private Jump_Movimiento playerMovement; // Cambio a Jump_Movimiento

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFishing)
        {
            playerInside = true;
            playerMovement = other.GetComponent<Jump_Movimiento>(); // Ahora busca Jump_Movimiento

            if (playerMovement != null)
            {
                playerMovement.enabled = false; // Desactiva el movimiento al entrar en la zona de pesca
            }

            StartFishing();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            isFishing = false;

            if (playerMovement != null)
            {
                playerMovement.enabled = true; // Reactiva el movimiento al salir
            }
        }
    }

    private void StartFishing()
    {
        isFishing = true;
        fishingDelay = Random.Range(2f, 20f);
        Debug.Log("Esperando a que pique un pez...");

        StartCoroutine(FishingCoroutine());
    }

    private IEnumerator FishingCoroutine()
    {
        float elapsedTime = 0f;

        // Esperar hasta que el pez pique
        while (elapsedTime < fishingDelay)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("¡Demasiado pronto! El pez se escapó...");
                EndFishing();
                yield break; // Sale inmediatamente de la corrutina
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (playerInside)
        {
            Debug.Log("¡Está picando! Pulsa 'W' o 'UP' en 0.7 segundos.");
            canCatchFish = true;

            yield return new WaitForSeconds(0.7f);

            if (canCatchFish)
            {
                Debug.Log("El pez se escapó...");
                EndFishing();
            }
        }
    }

    private void Update()
    {
        if (canCatchFish && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Debug.Log("¡Has pescado un pez!");
            canCatchFish = false;
            EndFishing();
        }
    }

    private void EndFishing()
    {
        isFishing = false;
        canCatchFish = false;

        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Reactiva el movimiento después de pescar
        }
    }
}
*/



/*
using System.Collections;
using UnityEngine;

public class FishingZone : MonoBehaviour
{
    private bool isFishing = false;
    private bool canCatchFish = false;
    private float fishingDelay;
    private bool playerInside = false;
    private Jump_Movimiento playerMovement; // Cambio a Jump_Movimiento

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFishing)
        {
            playerInside = true;
            playerMovement = other.GetComponent<Jump_Movimiento>(); // Ahora busca Jump_Movimiento

            if (playerMovement != null)
            {
                playerMovement.enabled = false; // Desactiva el movimiento al entrar en la zona de pesca
            }

            StartFishing();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            isFishing = false;

            if (playerMovement != null)
            {
                playerMovement.enabled = true; // Reactiva el movimiento al salir
            }
        }
    }

    private void StartFishing()
    {
        isFishing = true;
        fishingDelay = Random.Range(2f, 20f);
        Debug.Log("Esperando a que pique un pez...");

        StartCoroutine(FishingCoroutine());
    }

    private IEnumerator FishingCoroutine()
    {
        yield return new WaitForSeconds(fishingDelay);

        if (playerInside)
        {
            Debug.Log("¡Está picando! Pulsa 'W' o 'UP' en 1 segundo.");
            canCatchFish = true;

            yield return new WaitForSeconds(1f);

            if (canCatchFish)
            {
                Debug.Log("El pez se escapó...");
                EndFishing();
            }
        }
    }

    private void Update()
    {
        if (canCatchFish && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Debug.Log("¡Has pescado un pez!");
            canCatchFish = false;
            EndFishing();
        }
    }

    private void EndFishing()
    {
        isFishing = false;
        canCatchFish = false;

        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Reactiva el movimiento después de pescar
        }
    }
}
 
*/
