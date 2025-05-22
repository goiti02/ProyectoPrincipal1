using UnityEngine;

public class DetectarBorde : MonoBehaviour
{
    [SerializeField] private Transform puntoIzquierdo;
    [SerializeField] private Transform puntoDerecho;
    [SerializeField] private float distanciaRayo = 1.0f;
    [SerializeField] private LayerMask capaSuelo;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (puntoIzquierdo == null || puntoDerecho == null)
        {
            Debug.LogError("Debes asignar los puntos laterales en el Inspector.");
            enabled = false; // Desactivamos el script si no est�n los puntos asignados
        }
        if (rb == null)
        {
            Debug.LogError("Este script necesita un componente Rigidbody2D en el mismo GameObject.");
            enabled = false; // Desactivamos el script si no hay Rigidbody2D
        }
    }

    void Update()
    {
        bool izquierdaTocaSuelo = Physics2D.Raycast(puntoIzquierdo.position, Vector2.down, distanciaRayo, capaSuelo);
        bool derechaTocaSuelo = Physics2D.Raycast(puntoDerecho.position, Vector2.down, distanciaRayo, capaSuelo);

        // Dibujamos los rayos para visualizaci�n en la escena
        Debug.DrawRay(puntoIzquierdo.position, Vector2.down * distanciaRayo, izquierdaTocaSuelo ? Color.green : Color.red);
        Debug.DrawRay(puntoDerecho.position, Vector2.down * distanciaRayo, derechaTocaSuelo ? Color.green : Color.red);

        // Comprobamos si alguno de los rayos no toca el suelo y el personaje est� quieto
        if ((!izquierdaTocaSuelo || !derechaTocaSuelo) && rb.velocity == Vector2.zero)
        {
            Debug.Log("Animaci�n: Me voy a caer.");
            // Aqu� podr�as activar la animaci�n correspondiente en tu Animator
        }
    }
}