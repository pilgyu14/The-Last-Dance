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
    /// ���� ������ �� ���� �ð����� ������ ���� �� �ֵ��� 
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
