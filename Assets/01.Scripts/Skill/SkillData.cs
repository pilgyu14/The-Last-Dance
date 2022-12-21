using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �нú� ��ų �θ� 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SkillData<T> : MonoBehaviour where T : IAgent
{
    protected T _owner; 

    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
