using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// �̵� ���� ����( �ִ�ӵ�, ���ӵ� ���ӵ� ) 
public class MovementInfo
{
    [Header("������ ���� ")]
    public float maxSpeed;

    public float acceleration;
    public float deacceleration;

    public float rotSpeed; 
}


[CreateAssetMenu(menuName = "SO/Agent/AgentAbilitySO")]
public class AgentSO : ScriptableObject
{
    public float hp;

    public MovementInfo moveInfo; 
}
