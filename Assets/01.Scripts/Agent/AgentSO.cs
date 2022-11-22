using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// �̵� ���� ����( �ִ�ӵ�, ���ӵ� ���ӵ� ) 
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
