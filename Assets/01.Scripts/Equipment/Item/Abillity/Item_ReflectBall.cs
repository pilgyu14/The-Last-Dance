using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ReflectBall : Item_Base
{
    public int reflectCnt = 3;

    public override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Monster") || col.gameObject.CompareTag("Wall"))
        {
            if (col.gameObject.CompareTag("Monster"))
            {
                Attack(col.gameObject);
            }

            reflectCnt--;
            Vector3 incomingVector = rigid.velocity;

            Vector3 normalVector = col.contacts[0].normal;

            Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector);
            reflectVector = reflectVector.normalized;

            rigid.velocity = reflectVector * speed;
        }
    }

    public override void Attack(GameObject monster)
    {
        if(reflectCnt <= 0)
        {
            base.Attack(monster);
            return;
        }
        //monster.GetComponent<Hp?>().Damage(damage);
    }
}
