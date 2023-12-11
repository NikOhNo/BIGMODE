using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadNextScene() 
    {
        int currIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("Reached end of scenes, returning to scene 0");
            SceneManager.LoadScene(0);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
    }
}
