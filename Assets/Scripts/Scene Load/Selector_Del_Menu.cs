using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selector_Del_Menu : MonoBehaviour
{
    public void IndexLoad(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex <= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Revisa el indice");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("*Se cierra la DEMO*");
    }
}
