using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 0;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Attack(other.gameObject);
        }
    }

    public virtual void Attack(GameObject monster)
    {
        //monster.GetComponent<Hp?>().Damage(damage);
        Destroy(gameObject);
    }
}
