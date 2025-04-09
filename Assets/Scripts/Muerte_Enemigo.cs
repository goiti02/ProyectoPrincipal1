using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muerte_Enemigo : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }

}
