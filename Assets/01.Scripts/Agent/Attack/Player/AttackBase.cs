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
    public UnityEvent feedbackCallback = null; // �⺻���� �ǵ�� ( ������ �� ���� ) , ��ų ������ �� �ٸ� �ൿ( ��Ŭ�� ��� -> �����̵� ������ ) 
    public UnityEvent feedbackCallbackHit = null; // ������ �� ���� �ǵ�� ( ī�޶� ��鸲 ) 
}


[System.Serializable]
public class AttackBase 
{
    // ĳ�� ���� 
    private IAgent _owner; 

    private PlayerAnimation _agentAnimation;
    // ���� ��� ����
    // attackSO.IsEnemy �ִ� 

    private AttackJudgementComponent _atkJudgeComponent; 
    private PlayerMoveModule _moveModule;
    private FieldOfView _fov;

    // �ν����� ���� ���� 
    public AttackInfo attackInfo;
    public AttackCollider attackCollider;

    // ������Ƽ 
    public AttackJudgementComponent AtkJudgeComponent => _atkJudgeComponent; 

    public void Init(IAgent owner, PlayerAnimation playerAnimation, PlayerMoveModule moveModule,FieldOfView fov)
    {
        _owner = owner; 
        _agentAnimation = playerAnimation;
        _moveModule = moveModule;
        _fov = fov;

        _atkJudgeComponent = new AttackJudgementComponent();
        _atkJudgeComponent.Init(owner, attackInfo);
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
        ////
        if(attackInfo.attackSO.isRayAttack == true)
        {
            FindTarget(); // Ÿ�� ã�� ���� �ǵ�� ���� 
        }
        else
        {
            attackCollider.ActiveCollider(true, true);
        }
        ////


        if (_fov.TargetList.Count >= 1)
        {
            attackInfo.feedbackCallbackHit?.Invoke(); 
        }
        // ��? 
        _agentAnimation.Update_Zero();

        attackInfo.feedbackCallback?.Invoke(); 
    }

    private void FindTarget()
    {
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // ���� �� ���� ã�� 

        foreach (var target in _fov.TargetList) // Ÿ�� ���� ����( ������ , �˹� )  
        {
            // ���⸦ �� ó���ؾ���
            _atkJudgeComponent.AttackJudge(target);
        }
    }

    public void ActiveEffect()
    {

    }
    
}

