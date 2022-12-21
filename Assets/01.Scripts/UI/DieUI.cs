using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieUI : MonoBehaviour
{
    public void OnMainButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
