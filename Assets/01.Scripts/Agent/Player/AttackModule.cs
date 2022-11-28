using UnityEngine;
using System;
using System.Collections.Generic;

public enum AttackType
{
    Default_1,
    Default_2,
    Default_3,
}


public class AttackModule : MonoBehaviour
{

    [SerializeField]
    private bool isEnemy; 
    [SerializeField]
    private List<AttackInfo> attackInfoList = new List<AttackInfo>();


    private AttackInfo _curAttackInfo;
    [SerializeField]
    private LayerMask _hitLayerMask;

    // 프로퍼티
    public AttackSO curAttackSO => _curAttackInfo.attackSO; 
    private void Start()
    {
        _hitLayerMask =  (isEnemy) ? 1 << LayerMask.NameToLayer("Player") : 1 << LayerMask.NameToLayer("Enemy");
        _curAttackInfo = attackInfoList[0]; 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            DefaultAttack(); 
        }
    }
    public void Init()
    {

    }

    public void DefaultAttack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, curAttackSO.attackRadius, _hitLayerMask);
        List<Collider> targets= new List<Collider>(); 
        foreach(var col in cols)
        {
            Vector3 targetPos =(col.transform.position - transform.position).normalized; 
            float angle = Vector3.Angle(transform.position.normalized, targetPos);
            Debug.DrawLine(transform.position, col.transform.position,Color.black, 10f);
            if(angle < curAttackSO.attackAngle)
            {
                targets.Add(col);
                Debug.Log(col.name); 
            }
        }

        foreach(var target in targets)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable.GetDamaged(curAttackSO.attackDamage, gameObject); 
            // 데미지 텍스트 
            // 이펙트 
            // 흔들림 

            if (curAttackSO.isKnockbackAttack == true)
            {
                IKnockback knockback = target.GetComponent<IKnockback>();
            }
            
        }
    }


    private void OnDrawGizmos()
    {
        if (_curAttackInfo == null) _curAttackInfo = attackInfoList[0]; 
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, curAttackSO.attackRadius);

    }
}
