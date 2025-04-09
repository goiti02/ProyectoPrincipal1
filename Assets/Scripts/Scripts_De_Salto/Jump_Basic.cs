using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Basic : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpAmount = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Has pulsado space");
            rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        }
    }
}
