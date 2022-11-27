using UnityEngine;
using System;
using System.Collections.Generic;

public enum AttackType
{
    Default_1,
    Default_2,
}

public class AttackModule : MonoBehaviour
{
    [SerializeField]
    private List<AttackInfo> attackInfoList = new List<AttackInfo>(); 


    public void DefaultAttack()
    {

    }
}
