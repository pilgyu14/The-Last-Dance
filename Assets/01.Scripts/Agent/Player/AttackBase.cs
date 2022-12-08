using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FeedbackInfo
{
    // ī�޶� ��鸲 �ǵ��
    public float amplitude;
    public float intensity;
    public float shakeTime; 

    // 
}


[System.Serializable]
public class AttackInfo
{
    public AttackType attackType;
    public AttackType nextAttackType;
    public AttackSO attackSO;
    public UnityEvent feedbackCallback = null; // �⺻���� �ǵ�� ( ������ �� ���� ) 
    public UnityEvent feedbackCallbackHit = null; // ������ �� ���� �ǵ�� ( ī�޶� ��鸲 ) 
}


[System.Serializable]
public class AttackBase 
{
    // ĳ�� ���� 
    private GameObject _owner; 

    private PlayerAnimation _agentAnimation;
    // ���� ��� ����
    // attackSO.IsEnemy �ִ� 

    private PlayerMoveModule _moveModule;
    private FieldOfView _fov;

    // �ν����� ���� ���� 
    public AttackInfo attackInfo;

    public void Init(GameObject owner, PlayerAnimation playerAnimation, PlayerMoveModule moveModule,FieldOfView fov)
    {
        _owner = owner; 
        _agentAnimation = playerAnimation;
        _moveModule = moveModule;
        _fov = fov; 
    }

    /// <summary>
    /// �������� ���� ���� 
    /// </summary>
    public virtual void Attack() 
    {
        // ������ ���߰�
        _moveModule.StopMove();
        
        // �ִϸ��̼� ���� 
        if(attackInfo.attackSO.animationFuncName != null)
        {
            Type type = typeof(PlayerAnimation);
            MethodInfo method = type.GetMethod(attackInfo.attackSO.animationFuncName);
            method?.Invoke(_agentAnimation, new object[] { });
        }

        // ���� ���� 
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // ���� �� ���� ã�� 

        foreach (var target in _fov.TargetList) // Ÿ�� ���� ����( ������ , �˹� )  
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable.GetDamaged(attackInfo.attackSO.attackDamage, _owner);

            // ����Ʈ 
            Vector3 hitPos = target.position;
            // ���� ��ġ 
            // ��Ʈ ����� ��� 

            // �÷��̾� �����̶�� 
            // ������ �ؽ�Ʈ 
            DamageText damageText = new DamageText(); // Ǯ������ 
            damageText.SetText(attackInfo.attackSO.attackDamage, _owner.transform.position + new Vector3(0, 0.5f, 0), Color.white, false);
            
            // ��鸲 

            if (attackInfo.attackSO.isKnockbackAttack == true)
            {
                Vector3 dir = (target.position - _owner.transform.position).normalized;
                IKnockback knockback = target.GetComponent<IKnockback>();
                knockback.Knockback(dir, attackInfo.attackSO.knockbackPower, 0.2f); 
            }
        }
        
        if(_fov.TargetList.Count >= 1)
        {
            attackInfo.feedbackCallbackHit?.Invoke(); 
        }
        // ��? 
        _agentAnimation.Update_Zero();

        attackInfo.feedbackCallback?.Invoke(); 
    }

    
}

