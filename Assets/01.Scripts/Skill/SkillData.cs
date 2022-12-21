using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 패시브 스킬 부모 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SkillData<T> : MonoBehaviour where T : IAgent
{
    protected T _owner; 

    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}
