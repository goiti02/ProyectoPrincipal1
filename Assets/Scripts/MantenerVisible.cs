using UnityEngine;

public class MantenerVisible : MonoBehaviour
{
    private Renderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (myRenderer != null && !myRenderer.enabled)
        {
            myRenderer.enabled = true;
        }
    }
}
