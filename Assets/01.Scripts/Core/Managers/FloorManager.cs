using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    MainScene,
    LoadingScene, 
    Floor1Scene, 
    Floof2Scene,
    Floof3Scene,
    Floof4Scene,

}


[System.Serializable]
public class SceneInfo
{
    public SceneType sceneType; 
    public string sceneName; 
}


public class FloorManager : MonoBehaviour
{
    [SerializeField]
    private float _curFloor;
    [SerializeField]
    private List<SceneInfo> _sceneInfoList = new List<SceneInfo>(); 
    

}
