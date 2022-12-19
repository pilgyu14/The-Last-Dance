using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ExplosionBall : Item_Base
{
    //private HP monsterHP;
    [SerializeField]
    private float _explosionRadius; // 폭발 반경 

    public override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Monster") || col.gameObject.CompareTag("Wall"))
        {
            // 터지는 이펙트 
            Attack(null);
        }
    }

    public override void Attack(GameObject monster)
    {
        Collider[] monsters = Physics.OverlapSphere(transform.position, _explosionRadius, LayerMask.GetMask("Enemy"));

        for(int i = 0; i < monsters.Length; i++)
        {
            //monsterHP = monsters[i].GetComponent<HP>();
            //monsterHP.Damage(damage);
        }
        Destroy(gameObject);
    }
}
