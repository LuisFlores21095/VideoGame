using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        // No visible changes in the unity editor hence the debug.log to make sure it works 
        Debug.Log("Quit");
        Application.Quit();
    }
}
