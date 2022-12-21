using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : PoolableMono
{
    protected PlayerController _owner; // 아 바꿔야 해 (일단 임시로 ) 
    [SerializeField]
    protected AttackBase _attackBase;
    protected AttackJudgementComponent _attackJudgementComponent;

    public float speed = 5f;
    //public int damage = 0;


    protected Rigidbody rigid;

    private void Awake()
    {
        _owner = FindObjectOfType<PlayerController>(); 
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _attackJudgementComponent = new AttackJudgementComponent();
        _attackJudgementComponent.Init(_owner, _attackBase.attackInfo);
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

    [ContextMenu("테스트")]
    public void Test()
    {
        _attackJudgementComponent.AttackJudge(transform, _attackBase.attackCollider.transform);
    }
    public virtual void Attack(GameObject monster)
    {
        _attackJudgementComponent.AttackJudge(monster.transform, _attackBase.attackCollider.transform);
        //monster.GetComponent<IDamagable>().GetDamaged(damage,gameObject);
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
