using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    DefaultType,
    InBattleType,
}

public abstract class State<T>
{
    protected T owner;


    public virtual void Init(T owner)
    {
        this.owner = owner;
    }
    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();

}

