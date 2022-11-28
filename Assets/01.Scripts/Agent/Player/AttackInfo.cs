using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInfo 
{
    public AttackType attackType;
    public AttackSO attackSO;

    public virtual void Attack() { } 
}

