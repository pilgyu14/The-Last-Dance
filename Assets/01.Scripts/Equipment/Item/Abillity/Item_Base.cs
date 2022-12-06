using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : PoolableMono
{
    public float speed = 5f;
    public int damage = 0;

    public Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Monster"))
        {
            Attack(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Wall"))
        {
            PoolManager.Instance.Push(this);
        }
    }

    public virtual void Attack(GameObject monster)
    {
        //monster.GetComponent<Hp?>().Damage(damage);
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        rigid.velocity = transform.forward * speed;
    }

    public void SetPosAndRot(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
        Reset();
    }
}
