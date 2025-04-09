using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampas : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //que si colisiona con otro triger
    {
    //DA FALLO
    //other.GetComponent<Script_Jugador>().Damage(trampa); //Que ejecute el componente damage del script Script_Jugador
        Debug.Log("He estrado en el trigger de dtrampa"); //Debug para saber que esta detectando bien que estas pasando por la zona trampa

        //var player = other.GetComponent<>

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
