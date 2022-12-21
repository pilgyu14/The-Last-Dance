using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvasUI : MonoBehaviour
{
    public void GoMainScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
