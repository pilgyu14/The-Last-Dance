using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PenetrateBall : Item_Base
{
    public int penetrateCnt = 3;
    private bool isPenetrate = false;

    public override void OnCollisionEnter(Collision col)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") && isPenetrate == false)
        {
            isPenetrate = true;
            penetrateCnt--;
            Attack(other.gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            PoolManager.Instance.Push(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPenetrate = false;
    }

    public override void Attack(GameObject monster)
    {
        if(penetrateCnt <= 0)
        {
            base.Attack(monster);
            return;
        }
        _attackJudgementComponent.AttackJudge(monster.transform);
        //monster.GetComponent<Hp?>().Damage(damage);
    }

    public override void Reset()
    {
        base.Reset();
        penetrateCnt = 3;
        isPenetrate = false;
    }
}
