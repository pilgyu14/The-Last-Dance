using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvasUI : MonoBehaviour
{
    public void GoMainScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void GameQuit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
