using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextScene = "UnitTest";
    public void StartGame()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
