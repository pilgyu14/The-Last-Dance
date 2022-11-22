using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 이동 관련 정보( 최대속도, 가속도 감속도 ) 
public class MovementInfo
{
    public float maxSpeed;

    public float acceleration;
    public float deacceleration; 
}


[CreateAssetMenu(menuName = "SO/Agent/AgentAbilitySO")]
public class AgentSO : ScriptableObject
{
    public float hp;

    public MovementInfo moveInfo; 
}
