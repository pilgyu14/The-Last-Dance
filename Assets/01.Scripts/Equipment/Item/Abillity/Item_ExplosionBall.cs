using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ExplosionBall : Item_Base
{
    //private HP monsterHP;
    [SerializeField]
    private float _explosionRadius; // Æø¹ß ¹Ý°æ 

    public override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Monster") || col.gameObject.CompareTag("Wall"))
        {
            PoolableMono explosion = PoolManager.Instance.Pop("Explosion8");
            explosion.transform.position = transform.position;

            Attack(null);
        }
    }

    public override void Attack(GameObject monster)
    {
        Collider[] monsters = Physics.OverlapSphere(transform.position, _explosionRadius, LayerMask.GetMask("Enemy"));

        for(int i = 0; i < monsters.Length; i++)
        {
            _attackJudgementComponent.AttackJudge(monsters[i].transform);

            //monsterHP = monsters[i].GetComponent<HP>();
            //monsterHP.Damage(damage);
        }
        PoolManager.Instance.Push(this);
    }
}
