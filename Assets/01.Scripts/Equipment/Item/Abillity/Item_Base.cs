using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 0;

    public Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        rigid.velocity = transform.forward * speed;
    }

    public virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Monster"))
        {
            Attack(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public virtual void Attack(GameObject monster)
    {
        //monster.GetComponent<Hp?>().Damage(damage);
        Destroy(gameObject);
    }
}
