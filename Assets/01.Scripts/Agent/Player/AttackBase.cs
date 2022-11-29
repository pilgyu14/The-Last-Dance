using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class AttackInfo
{
    public AttackType attackType;
    public AttackSO attackSO;
}


[System.Serializable]
public class AttackBase 
{
    // ĳ�� ���� 
    private GameObject _owner; 

    private PlayerAnimation _agentAnimation;
    // ���� ��� ����
    // attackSO.IsEnemy �ִ� 

    private MoveModule _moveModule;
    private FieldOfView _fov;

    // �ν����� ���� ���� 
    public AttackInfo attackInfo; 

    public void Init(GameObject owner, PlayerAnimation playerAnimation, MoveModule moveModule,FieldOfView fov)
    {
        _owner = owner; 
        _agentAnimation = playerAnimation;
        _moveModule = moveModule;
        _fov = fov; 
    }
    public virtual void Attack() 
    {
        // ������ ���߰�
        _moveModule.StopMove();
        
        // �ִϸ��̼� ���� 
        if(attackInfo.attackSO.animationFuncName != null)
        {
            Type type = typeof(PlayerAnimation);
            MethodInfo method = type.GetMethod(attackInfo.attackSO.animationFuncName, BindingFlags.Static | BindingFlags.Public);
            method?.Invoke(_agentAnimation, new object[] { });
        }

        // ���� ���� 
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // ���� �� ���� ã�� 

        foreach (var target in _fov.TargetList)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable.GetDamaged(attackInfo.attackSO.attackDamage, _owner);
            // ������ �ؽ�Ʈ 
            // ����Ʈ 
            // ��鸲 

            if (attackInfo.attackSO.isKnockbackAttack == true)
            {
                IKnockback knockback = target.GetComponent<IKnockback>();
            }
        }

        // ��? 
        _agentAnimation.Update_Zero(); 
    }

    
}

