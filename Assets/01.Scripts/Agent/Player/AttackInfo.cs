using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackInfo 
{
    public AttackType attackType;
    public AttackSO attackSO;

    public abstract void Attack(); 
}

