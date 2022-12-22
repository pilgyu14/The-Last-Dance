using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.StartListening(EventsType.LoadMainScene, () => gameObject.SetActive(true));
    }
    public void OnPlayButtion()
    {
        SceneManager.LoadScene("BlueDungeon Game_Scene");
    }

    public void OnSettingButton()
    {
        //����â ����
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
