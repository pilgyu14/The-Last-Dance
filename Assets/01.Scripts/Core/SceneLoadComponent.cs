using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneLoadComponenet : MonoBehaviour
{

    private void Awake()
    {
        EventManager.Instance.StartListening(EventsType.LoadMainScene, SceneLoadMain);
        EventManager.Instance.StartListening(EventsType.LoadFloor1Scene, SceneLoadBattle);
        EventManager.Instance.StartListening(EventsType.LoadFloor2Scene, SceneLoadStageMake);
    }
    /// <summary>
    /// ��Ʋ���� �ε����� ���� �̵��Ѵ�
    /// </summary>
    public void SceneLoadBattle()
    {
        //if (_userDeckDataComponent != null)
        //{
        //    if (!_userDeckDataComponent.CheckCanPlayGame())
        //    {
        //        return;
        //    }
        //}
        SceneLoadBase();
        LoadingManager.LoadScene("BattleSceneRework");
    }
    /// <summary>
    /// ���ξ��� �ε����� ���� �̵��Ѵ�
    /// </summary>
    public void SceneLoadMain()
    {
        SceneLoadBase();
        LoadingManager.LoadScene("MainSceneRework");
    }
    /// <summary>
    /// �������� ���ۿ���� �ε����� ���� �̵��Ѵ�
    /// </summary>
    public void SceneLoadStageMake()
    {
        SceneLoadBase();
        LoadingManager.LoadScene("StageMakeScene");
    }

    /// <summary>
    /// ��κ��� ���� �̵��� �� �������� �κ�
    /// </summary>
    public void SceneLoadBase()
    {
        //��� ��Ʈ�� ����
        DOTween.KillAll();
        //�ð��� 1�� �ǵ���
        Time.timeScale = 1f;
        //���̺� �����Ϳ� �����ϴ� ������Ʈ�� ����
     //   UserSaveManagerSO.ClearObserver();
    }
}
