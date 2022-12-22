using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class DieCanvas : MonoBehaviour
{

    void Awake()
    {
        EventManager.Instance.StartListening(EventsType.LoadMainScene, () => gameObject.SetActive(true));
        gameObject.SetActive(false); 
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadBattle()
    {
        SceneManager.LoadScene("");
    }

}
