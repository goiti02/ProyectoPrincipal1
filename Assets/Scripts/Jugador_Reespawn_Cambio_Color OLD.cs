using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Jugador_Reespawn_Cambio_Color : MonoBehaviour
{
    [Header("Vida")]
    public float vida_Maxima_Jugador = 100f;
    public float vida_Jugador = 100f;
    public float ataque_Jugador = 20f;
    public float mana_Jugador = 40f;
    public float vida_Minima_Jugador = 0f;

    [Header("Damage")]
    public float damage_General = 10f;
    public float trampa = 10f;

    // Valor de daño para los pinchos - daño letal
    public float pinchoDamage = 100f;

    [Header("Movimiento del jugador")]
    public float moveSpeed = 3f;
    //private float aceleration = 5f;
    public float rotationSpeed = 720f;

    [Header("Pocion")]
    public bool pocion_bool = false;
    public int pocion_Cura;

    [Header("Sistema de Respawn")]
    // Array de modelos de personaje para ciclar a través de ellos
    public GameObject[] modelosPersonaje;
    private int indiceModeloActual = 0;

    // Punto de respawn actual
    private Vector3 ultimoPuntoRespawn;

    // Variable para guardar la coordenada Z inicial
    private float zInicial;

    // Referencias para solucionar problema de visibilidad
    private Camera mainCamera;
    private int originalCullingMask;
    private GameObject[] suelosEnEscena;

    //Variables de UI
    public TextMeshProUGUI Vida;

    // Start is called before the first frame update
    void Start()
    {
        PrintVida();

        // Guardar la coordenada Z inicial
        zInicial = transform.position.z;

        // Guardar la posición inicial como primer punto de respawn (manteniendo Z)
        ultimoPuntoRespawn = new Vector3(transform.position.x, transform.position.y, zInicial);

        // Verificar que tenemos modelos para usar
        if (modelosPersonaje.Length == 0)
        {
            Debug.LogWarning("No hay modelos de personaje asignados en el array!");
        }
        else
        {
            // Activar el primer modelo al iniciar
            ActivarModeloActual();
        }

        // Guardar referencia a la cámara y su configuración original
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCullingMask = mainCamera.cullingMask;
        }

        // Almacenar referencias a todos los suelos de la escena
        suelosEnEscena = GameObject.FindGameObjectsWithTag("Suelo");
        if (suelosEnEscena.Length == 0)
        {
            // Si no hay objetos con tag "Suelo", buscar por nombres comunes
            List<GameObject> suelosList = new List<GameObject>();
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains("suelo") ||
                    obj.name.ToLower().Contains("floor") ||
                    obj.name.ToLower().Contains("ground") ||
                    obj.name.ToLower().Contains("terrain"))
                {
                    suelosList.Add(obj);
                }
            }
            suelosEnEscena = suelosList.ToArray();
        }

        Debug.Log("Se encontraron " + suelosEnEscena.Length + " objetos de suelo en la escena");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
        PrintVida();

        // Forzar posición Z constante para juego 2D
        if (transform.position.z != zInicial)
        {
            Vector3 corregida = transform.position;
            corregida.z = zInicial;
            transform.position = corregida;
        }

        // Verificar que la cámara tenga la configuración correcta
        if (mainCamera != null && mainCamera.cullingMask != originalCullingMask)
        {
            mainCamera.cullingMask = originalCullingMask;
            Debug.Log("Restaurada configuración de culling mask de la cámara");
        }

        // Verificar que los suelos sean visibles
        foreach (GameObject suelo in suelosEnEscena)
        {
            if (suelo != null)
            {
                // Verificar que el objeto esté activo
                if (!suelo.activeSelf)
                {
                    suelo.SetActive(true);
                    Debug.Log("Reactivado suelo: " + suelo.name);
                }

                // Verificar que el renderer esté activo
                Renderer rend = suelo.GetComponent<Renderer>();
                if (rend != null && !rend.enabled)
                {
                    rend.enabled = true;
                    Debug.Log("Reactivado renderer de suelo: " + suelo.name);
                }
            }
        }
    }

    void PlayerInputs()
    {
        // Capturar inputs directos
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Modificar inputs para que izquierda implique también abajo, y derecha implique también arriba
        float modifiedVertical = moveVertical;

        // Si estamos moviendo horizontalmente y no hay input vertical explícito
        if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(moveVertical) < 0.1f)
        {
            // Izquierda (-) implica también abajo (-), derecha (+) implica también arriba (+)
            modifiedVertical = Mathf.Sign(moveHorizontal) * 0.5f; // Usar 0.5f para reducir la intensidad
        }

        // Combinar los inputs para el movimiento visual 3D pero manteniendo funcionamiento 2D
        Vector3 movement = new Vector3(moveHorizontal, modifiedVertical, 0);

        // Aplicar el movimiento manteniendo Z constante
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        newPosition.z = zInicial; // Asegurar que la Z siempre sea constante
        transform.position = newPosition;

        // Añadir rotación visual solo si hay movimiento
        if (movement != Vector3.zero)
        {
            // Crear una rotación que mire hacia la dirección del movimiento pero solo en el plano X-Y
            // Esto mantiene el efecto visual 3D pero funcional para un juego 2D
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(movement.x, movement.y, 0), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void PrintVida()
    {
        Vida.text = vida_Jugador.ToString();
    }

    public void Vida_calc(int healAmount)
    {
        if (vida_Jugador + healAmount >= vida_Maxima_Jugador)
        {
            Debug.Log("Ya tienes la vida maxima");
            vida_Jugador = vida_Maxima_Jugador;
        }
        else
        {
            vida_Jugador = vida_Jugador + healAmount;
            Debug.Log("Vida");
            Debug.Log("Despues de curarte la vida es:" + vida_Jugador);
        }
    }

    public void Damage(float damageAmount)
    {
        if (vida_Jugador - damageAmount <= vida_Minima_Jugador)
        {
            vida_Jugador = vida_Minima_Jugador;
            Debug.Log("Has muerto");

            // Si el daño mata al jugador, respawn
            Respawn();
        }
        else
        {
            vida_Jugador = vida_Jugador - damageAmount;
            Debug.Log("Has recibido daño. Vida actual: " + vida_Jugador);
        }
    }

    // Función para el respawn del jugador
    public void Respawn()
    {
        // Restaurar vida
        vida_Jugador = vida_Maxima_Jugador;

        // Teletransportar al punto de respawn manteniendo la Z fija
        Vector3 respawnPos = ultimoPuntoRespawn;
        respawnPos.z = zInicial; // Asegurar que la Z es la correcta
        transform.position = respawnPos;

        // Cambiar al siguiente modelo de personaje
        CambiarAlSiguienteModelo();

        // Asegurar que los suelos siguen siendo visibles después del respawn
        RestablecerVisibilidadSuelos();

        Debug.Log("Jugador respawneado en la posición: " + transform.position);
    }

    // Función para actualizar el punto de respawn
    public void ActualizarPuntoRespawn(Vector3 nuevaPosicion)
    {
        // Guardar nueva posición pero manteniendo la Z establecida
        ultimoPuntoRespawn = new Vector3(nuevaPosicion.x, nuevaPosicion.y, zInicial);
        Debug.Log("Nuevo punto de respawn establecido en: " + ultimoPuntoRespawn);
    }

    // Función para cambiar al siguiente modelo de personaje
    private void CambiarAlSiguienteModelo()
    {
        if (modelosPersonaje.Length > 0)
        {
            // Incrementar el índice (volver a 0 si llega al final)
            indiceModeloActual = (indiceModeloActual + 1) % modelosPersonaje.Length;
            ActivarModeloActual();
        }
    }

    // Activar el modelo actual y desactivar los demás
    private void ActivarModeloActual()
    {
        // Desactivar todos los modelos primero
        for (int i = 0; i < modelosPersonaje.Length; i++)
        {
            modelosPersonaje[i].SetActive(false);
        }

        // Activar solo el modelo actual
        modelosPersonaje[indiceModeloActual].SetActive(true);
        Debug.Log("Cambiado al modelo de personaje: " + indiceModeloActual);
    }

    // Función para asegurar que los suelos son visibles
    private void RestablecerVisibilidadSuelos()
    {
        // Restaurar configuración de la cámara
        if (mainCamera != null)
        {
            mainCamera.cullingMask = originalCullingMask;
        }

        // Asegurar que todos los suelos están activos y visibles
        foreach (GameObject suelo in suelosEnEscena)
        {
            if (suelo != null)
            {
                suelo.SetActive(true);

                Renderer rend = suelo.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.enabled = true;
                }
            }
        }

        Debug.Log("Visibilidad de suelos restablecida");
    }

    public void Potion_Status(bool potionStatus)
    {
        Debug.Log("En teoria deberia funcionar y ponerse en False el bool de pocion.");
        Debug.Log("en teoria se esta ejecutando lo de pocion_booL=false");
    }

    public void Boton_Damage()
    {
        Damage(damage_General);
    }

    public void Boton_Vida()
    {
        // Vida_calc(cura_General);
    }

    public void Boton_Pocion()
    {
        if (pocion_bool == true)
        {
            Vida_calc(pocion_Cura);
            pocion_bool = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si es una trampa normal
        if (other.CompareTag("Trampa"))
        {
            Damage(trampa);
            Debug.Log("He entrado en el trigger de trampa");
        }
        // Detectar si son pinchos (trampa mortal)
        else if (other.CompareTag("Pinchos"))
        {
            Debug.Log("¡Has caído en pinchos! Muerte instantánea.");
            // Aplicar daño letal
            Damage(pinchoDamage);
        }
        // Detectar si es un checkpoint
        else if (other.CompareTag("Checkpoint"))
        {
            // Actualizar el punto de respawn
            ActualizarPuntoRespawn(other.transform.position);
        }

        // Asegurar que el suelo sigue siendo visible después de cualquier interacción con triggers
        RestablecerVisibilidadSuelos();
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar visibilidad de suelos al salir de cualquier trigger
        RestablecerVisibilidadSuelos();

        // Información de debug
        Debug.Log("Saliendo del trigger: " + other.tag);
        if (mainCamera != null)
        {
            Debug.Log("Estado de la cámara: Culling Mask = " + mainCamera.cullingMask);
        }
    }
}



/*

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Jugador_Reespawn_Cambio_Color : MonoBehaviour
{
    [Header("Vida")]
    public float vida_Maxima_Jugador = 100f;
    public float vida_Jugador = 100f;
    public float ataque_Jugador = 20f;
    public float mana_Jugador = 40f;
    public float vida_Minima_Jugador = 0f;

    [Header("Damage")]
    public float damage_General = 10f;
    public float trampa = 10f;

    // Valor de daño para los pinchos - daño letal
    public float pinchoDamage = 100f;

    [Header("Movimiento del jugador")]
    public float moveSpeed = 3f;
    private float aceleration = 5f;
    public float rotationSpeed = 720f;

    [Header("Configuración 2D")]
    public float zPosition = 0f; // Mantener posición Z fija para juego 2D

    [Header("Pocion")]
    public bool pocion_bool = false;
    public int pocion_Cura;

    [Header("Sistema de Respawn")]
    // Array de modelos de personaje para ciclar a través de ellos
    public GameObject[] modelosPersonaje;
    private int indiceModeloActual = 0;

    // Punto de respawn actual
    private Vector3 ultimoPuntoRespawn;

    // Referencias para solucionar problema de visibilidad
    private Camera mainCamera;
    private int originalCullingMask;
    private GameObject[] suelosEnEscena;

    //Variables de UI
    public TextMeshProUGUI Vida;

    // Start is called before the first frame update
    void Start()
    {
        PrintVida();

        // Guardar la posición inicial como primer punto de respawn
        ultimoPuntoRespawn = new Vector3(transform.position.x, transform.position.y, zPosition);

        // Verificar que tenemos modelos para usar
        if (modelosPersonaje.Length == 0)
        {
            Debug.LogWarning("No hay modelos de personaje asignados en el array!");
        }
        else
        {
            // Activar el primer modelo al iniciar
            ActivarModeloActual();
        }

        // Guardar referencia a la cámara y su configuración original
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCullingMask = mainCamera.cullingMask;
        }

        // Almacenar referencias a todos los suelos de la escena
        suelosEnEscena = GameObject.FindGameObjectsWithTag("Suelo");
        if (suelosEnEscena.Length == 0)
        {
            // Si no hay objetos con tag "Suelo", buscar por nombres comunes
            List<GameObject> suelosList = new List<GameObject>();
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains("suelo") ||
                    obj.name.ToLower().Contains("floor") ||
                    obj.name.ToLower().Contains("ground") ||
                    obj.name.ToLower().Contains("terrain"))
                {
                    suelosList.Add(obj);
                }
            }
            suelosEnEscena = suelosList.ToArray();
        }

        Debug.Log("Se encontraron " + suelosEnEscena.Length + " objetos de suelo en la escena");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
        PrintVida();

        // Forzar posición Z constante para juego 2D
        if (transform.position.z != zPosition)
        {
            Vector3 corregida = transform.position;
            corregida.z = zPosition;
            transform.position = corregida;
        }

        // Verificar que la cámara tenga la configuración correcta
        if (mainCamera != null && mainCamera.cullingMask != originalCullingMask)
        {
            mainCamera.cullingMask = originalCullingMask;
            Debug.Log("Restaurada configuración de culling mask de la cámara");
        }

        // Verificar que los suelos sean visibles
        foreach (GameObject suelo in suelosEnEscena)
        {
            if (suelo != null)
            {
                // Verificar que el objeto esté activo
                if (!suelo.activeSelf)
                {
                    suelo.SetActive(true);
                    Debug.Log("Reactivado suelo: " + suelo.name);
                }

                // Verificar que el renderer esté activo
                Renderer rend = suelo.GetComponent<Renderer>();
                if (rend != null && !rend.enabled)
                {
                    rend.enabled = true;
                    Debug.Log("Reactivado renderer de suelo: " + suelo.name);
                }
            }
        }
    }

    void PlayerInputs()
    {
        // Capturar inputs directos
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Modificar inputs para que izquierda implique también abajo, y derecha implique también arriba
        float modifiedVertical = moveVertical;

        // Si estamos moviendo horizontalmente y no hay input vertical explícito
        if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(moveVertical) < 0.1f)
        {
            // Izquierda (-) implica también abajo (-), derecha (+) implica también arriba (+)
            modifiedVertical = Mathf.Sign(moveHorizontal) * 0.5f; // Usar 0.5f para reducir la intensidad
        }

        // Combinar los inputs para el movimiento visual 3D pero manteniendo funcionamiento 2D
        Vector3 movement = new Vector3(moveHorizontal, modifiedVertical, 0);

        // Aplicar el movimiento manteniendo Z constante
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        newPosition.z = zPosition; // Asegurar que la Z siempre sea constante
        transform.position = newPosition;

        // Añadir rotación visual solo si hay movimiento
        if (movement != Vector3.zero)
        {
            // Crear una rotación que mire hacia la dirección del movimiento pero solo en el plano X-Y
            // Esto mantiene el efecto visual 3D pero funcional para un juego 2D
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(movement.x, movement.y, 0), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void PrintVida()
    {
        Vida.text = vida_Jugador.ToString();
    }

    public void Vida_calc(int healAmount)
    {
        if (vida_Jugador + healAmount >= vida_Maxima_Jugador)
        {
            Debug.Log("Ya tienes la vida maxima");
            vida_Jugador = vida_Maxima_Jugador;
        }
        else
        {
            vida_Jugador = vida_Jugador + healAmount;
            Debug.Log("Vida");
            Debug.Log("Despues de curarte la vida es:" + vida_Jugador);
        }
    }

    public void Damage(float damageAmount)
    {
        if (vida_Jugador - damageAmount <= vida_Minima_Jugador)
        {
            vida_Jugador = vida_Minima_Jugador;
            Debug.Log("Has muerto");

            // Si el daño mata al jugador, respawn
            Respawn();
        }
        else
        {
            vida_Jugador = vida_Jugador - damageAmount;
            Debug.Log("Has recibido daño. Vida actual: " + vida_Jugador);
        }
    }

    // Función para el respawn del jugador
    public void Respawn()
    {
        // Restaurar vida
        vida_Jugador = vida_Maxima_Jugador;

        // Teletransportar al punto de respawn manteniendo la Z fija
        Vector3 respawnPos = ultimoPuntoRespawn;
        respawnPos.z = zPosition; // Asegurar que la Z es la correcta
        transform.position = respawnPos;

        // Cambiar al siguiente modelo de personaje
        CambiarAlSiguienteModelo();

        // Asegurar que los suelos siguen siendo visibles después del respawn
        RestablecerVisibilidadSuelos();

        Debug.Log("Jugador respawneado en la posición: " + transform.position);
    }

    // Función para actualizar el punto de respawn
    public void ActualizarPuntoRespawn(Vector3 nuevaPosicion)
    {
        // Guardar nueva posición pero manteniendo la Z establecida
        ultimoPuntoRespawn = new Vector3(nuevaPosicion.x, nuevaPosicion.y, zPosition);
        Debug.Log("Nuevo punto de respawn establecido en: " + ultimoPuntoRespawn);
    }

    // Función para cambiar al siguiente modelo de personaje
    private void CambiarAlSiguienteModelo()
    {
        if (modelosPersonaje.Length > 0)
        {
            // Incrementar el índice (volver a 0 si llega al final)
            indiceModeloActual = (indiceModeloActual + 1) % modelosPersonaje.Length;
            ActivarModeloActual();
        }
    }

    // Activar el modelo actual y desactivar los demás
    private void ActivarModeloActual()
    {
        // Desactivar todos los modelos primero
        for (int i = 0; i < modelosPersonaje.Length; i++)
        {
            modelosPersonaje[i].SetActive(false);
        }

        // Activar solo el modelo actual
        modelosPersonaje[indiceModeloActual].SetActive(true);
        Debug.Log("Cambiado al modelo de personaje: " + indiceModeloActual);
    }

    // Función para asegurar que los suelos son visibles
    private void RestablecerVisibilidadSuelos()
    {
        // Restaurar configuración de la cámara
        if (mainCamera != null)
        {
            mainCamera.cullingMask = originalCullingMask;
        }

        // Asegurar que todos los suelos están activos y visibles
        foreach (GameObject suelo in suelosEnEscena)
        {
            if (suelo != null)
            {
                suelo.SetActive(true);

                Renderer rend = suelo.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.enabled = true;
                }
            }
        }

        Debug.Log("Visibilidad de suelos restablecida");
    }

    public void Potion_Status(bool potionStatus)
    {
        Debug.Log("En teoria deberia funcionar y ponerse en False el bool de pocion.");
        Debug.Log("en teoria se esta ejecutando lo de pocion_booL=false");
    }

    public void Boton_Damage()
    {
        Damage(damage_General);
    }

    public void Boton_Vida()
    {
        // Vida_calc(cura_General);
    }

    public void Boton_Pocion()
    {
        if (pocion_bool == true)
        {
            Vida_calc(pocion_Cura);
            pocion_bool = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si es una trampa normal
        if (other.CompareTag("Trampa"))
        {
            Damage(trampa);
            Debug.Log("He entrado en el trigger de trampa");
        }
        // Detectar si son pinchos (trampa mortal)
        else if (other.CompareTag("Pinchos"))
        {
            Debug.Log("¡Has caído en pinchos! Muerte instantánea.");
            // Aplicar daño letal
            Damage(pinchoDamage);
        }
        // Detectar si es un checkpoint
        else if (other.CompareTag("Checkpoint"))
        {
            // Actualizar el punto de respawn
            ActualizarPuntoRespawn(other.transform.position);
        }

        // Asegurar que el suelo sigue siendo visible después de cualquier interacción con triggers
        RestablecerVisibilidadSuelos();
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar visibilidad de suelos al salir de cualquier trigger
        RestablecerVisibilidadSuelos();

        // Información de debug
        Debug.Log("Saliendo del trigger: " + other.tag);
        if (mainCamera != null)
        {
            Debug.Log("Estado de la cámara: Culling Mask = " + mainCamera.cullingMask);
        }
    }
}












*/

/* 
 * 
 * PAPER MARIO
 * ============
 * ============
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Jugador_Reespawn_Cambio_Color : MonoBehaviour
{
    [Header("Vida")]
    public float vida_Maxima_Jugador = 100f;
    public float vida_Jugador = 100f;
    public float ataque_Jugador = 20f;
    public float mana_Jugador = 40f;
    public float vida_Minima_Jugador = 0f;

    [Header("Damage")]
    public float damage_General = 10f;
    public float trampa = 10f;

    // Valor de daño para los pinchos - daño letal
    public float pinchoDamage = 100f;

    [Header("Movimiento del jugador")]
    public float moveSpeed = 3f;
    private float aceleration = 5f;
    public float rotationSpeed = 720f;

    [Header("Pocion")]
    public bool pocion_bool = false;
    public int pocion_Cura;

    [Header("Sistema de Respawn")]
    // Array de modelos de personaje para ciclar a través de ellos
    public GameObject[] modelosPersonaje;
    private int indiceModeloActual = 0;

    // Punto de respawn actual
    private Vector3 ultimoPuntoRespawn;
    // Referencia al modelo actual para poder cambiarlo
   // public Transform contenedorModelo;

    // Referencias para solucionar problema de visibilidad
    private Camera mainCamera;
    private int originalCullingMask;
    private GameObject[] suelosEnEscena;

    //Variables de UI
    public TextMeshProUGUI Vida;

    // Start is called before the first frame update
    void Start()
    {
        PrintVida();

        // Guardar la posición inicial como primer punto de respawn
        ultimoPuntoRespawn = transform.position;

        // Verificar que tenemos modelos para usar
        if (modelosPersonaje.Length == 0)
        {
            Debug.LogWarning("No hay modelos de personaje asignados en el array!");
        }
        else
        {
            // Activar el primer modelo al iniciar
            ActivarModeloActual();
        }

        // Guardar referencia a la cámara y su configuración original
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCullingMask = mainCamera.cullingMask;
        }

        // Almacenar referencias a todos los suelos de la escena
        suelosEnEscena = GameObject.FindGameObjectsWithTag("Suelo");
        if (suelosEnEscena.Length == 0)
        {
            // Si no hay objetos con tag "Suelo", buscar por nombres comunes
            List<GameObject> suelosList = new List<GameObject>();
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains("suelo") ||
                    obj.name.ToLower().Contains("floor") ||
                    obj.name.ToLower().Contains("ground") ||
                    obj.name.ToLower().Contains("terrain"))
                {
                    suelosList.Add(obj);
                }
            }
            suelosEnEscena = suelosList.ToArray();
        }

        Debug.Log("Se encontraron " + suelosEnEscena.Length + " objetos de suelo en la escena");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
        PrintVida();

        // Verificar que la cámara tenga la configuración correcta
        if (mainCamera != null && mainCamera.cullingMask != originalCullingMask)
        {
            mainCamera.cullingMask = originalCullingMask;
            Debug.Log("Restaurada configuración de culling mask de la cámara");
        }

        // Verificar que los suelos sean visibles
        foreach (GameObject suelo in suelosEnEscena)
        {
            if (suelo != null)
            {
                // Verificar que el objeto esté activo
                if (!suelo.activeSelf)
                {
                    suelo.SetActive(true);
                    Debug.Log("Reactivado suelo: " + suelo.name);
                }

                // Verificar que el renderer esté activo
                Renderer rend = suelo.GetComponent<Renderer>();
                if (rend != null && !rend.enabled)
                {
                    rend.enabled = true;
                    Debug.Log("Reactivado renderer de suelo: " + suelo.name);
                }
            }
        }
    }

    void PlayerInputs()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVerticar = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVerticar);

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void PrintVida()
    {
        Vida.text = vida_Jugador.ToString();
    }

    public void Vida_calc(int healAmount)
    {
        if (vida_Jugador + healAmount >= vida_Maxima_Jugador)
        {
            Debug.Log("Ya tienes la vida maxima");
            vida_Jugador = vida_Maxima_Jugador;
        }
        else
        {
            vida_Jugador = vida_Jugador + healAmount;
            Debug.Log("Vida");
            Debug.Log("Despues de curarte la vida es:" + vida_Jugador);
        }
    }

    public void Damage(float damageAmount)
    {
        if (vida_Jugador - damageAmount <= vida_Minima_Jugador)
        {
            vida_Jugador = vida_Minima_Jugador;
            Debug.Log("Has muerto");

            // Si el daño mata al jugador, respawn
            Respawn();
        }
        else
        {
            vida_Jugador = vida_Jugador - damageAmount;
            Debug.Log("Has recibido daño. Vida actual: " + vida_Jugador);
        }
    }

    // Función para el respawn del jugador
    public void Respawn()
    {
        // Restaurar vida
        vida_Jugador = vida_Maxima_Jugador;

        // Teletransportar al punto de respawn
        transform.position = ultimoPuntoRespawn;

        // Cambiar al siguiente modelo de personaje
        CambiarAlSiguienteModelo();

        // Asegurar que los suelos siguen siendo visibles después del respawn
        RestablecerVisibilidadSuelos();

        Debug.Log("Jugador respawneado en la posición: " + ultimoPuntoRespawn);
    }

    // Función para actualizar el punto de respawn
    public void ActualizarPuntoRespawn(Vector3 nuevaPosicion)
    {
        ultimoPuntoRespawn = nuevaPosicion;
        Debug.Log("Nuevo punto de respawn establecido en: " + nuevaPosicion);
    }

    // Función para cambiar al siguiente modelo de personaje
    private void CambiarAlSiguienteModelo()
    {
        if (modelosPersonaje.Length > 0)
        {
            // Incrementar el índice (volver a 0 si llega al final)
            indiceModeloActual = (indiceModeloActual + 1) % modelosPersonaje.Length;
            ActivarModeloActual();
        }
    }

    // Activar el modelo actual y desactivar los demás
    private void ActivarModeloActual()
    {
        // Desactivar todos los modelos primero
        for (int i = 0; i < modelosPersonaje.Length; i++)
        {
            modelosPersonaje[i].SetActive(false);
        }

        // Activar solo el modelo actual
        modelosPersonaje[indiceModeloActual].SetActive(true);
        Debug.Log("Cambiado al modelo de personaje: " + indiceModeloActual);
    }

    // Función para asegurar que los suelos son visibles
    private void RestablecerVisibilidadSuelos()
    {
        // Restaurar configuración de la cámara
        if (mainCamera != null)
        {
            mainCamera.cullingMask = originalCullingMask;
        }

        // Asegurar que todos los suelos están activos y visibles
        foreach (GameObject suelo in suelosEnEscena)
        {
            if (suelo != null)
            {
                suelo.SetActive(true);

                Renderer rend = suelo.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.enabled = true;
                }
            }
        }

        Debug.Log("Visibilidad de suelos restablecida");
    }

    public void Potion_Status(bool potionStatus)
    {
        Debug.Log("En teoria deberia funcionar y ponerse en False el bool de pocion.");
        Debug.Log("en teoria se esta ejecutando lo de pocion_booL=false");
    }

    public void Boton_Damage()
    {
        Damage(damage_General);
    }

    public void Boton_Vida()
    {
       // Vida_calc(cura_General);
    }

    public void Boton_Pocion()
    {
        if (pocion_bool == true)
        {
            Vida_calc(pocion_Cura);
            pocion_bool = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si es una trampa normal
        if (other.CompareTag("Trampa"))
        {
            Damage(trampa);
            Debug.Log("He entrado en el trigger de trampa");
        }
        // Detectar si son pinchos (trampa mortal)
        else if (other.CompareTag("Pinchos"))
        {
            Debug.Log("¡Has caído en pinchos! Muerte instantánea.");
            // Aplicar daño letal
            Damage(pinchoDamage);
        }
        // Detectar si es un checkpoint
        else if (other.CompareTag("Checkpoint"))
        {
            // Actualizar el punto de respawn
            ActualizarPuntoRespawn(other.transform.position);
        }

        // Asegurar que el suelo sigue siendo visible después de cualquier interacción con triggers
        RestablecerVisibilidadSuelos();
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar visibilidad de suelos al salir de cualquier trigger
        RestablecerVisibilidadSuelos();

        // Información de debug
        Debug.Log("Saliendo del trigger: " + other.tag);
        if (mainCamera != null)
        {
            Debug.Log("Estado de la cámara: Culling Mask = " + mainCamera.cullingMask);
        }
    }
}

*/