using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueAttackCollider : AttackCollider
{

    public override void ActiveCollider(bool isActive, bool isContinue = false)
    {
        base.ActiveCollider(isActive, isContinue);
        StartCoroutine(AttackPossible()); 
    }

    public override void ColliderFalse()
    {
        base.ColliderFalse();
        StopCoroutine(AttackPossible()); 
    }
    /// <summary>
    /// 지속 공격일 때 일정 시간마다 데미지 입힐 수 있도록 
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackPossible()
    {
        while (true)
        {
            colObjs.Clear(); 
            yield return new WaitForSeconds(0.3f);
        }
    }
}
