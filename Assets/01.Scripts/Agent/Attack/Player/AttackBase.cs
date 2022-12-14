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
public class AttackBase : ICoolTime
{
    // ���� ���� 
    private bool _isCoolTime = false; // ��Ÿ�����ΰ� 
    private float _remainTime;  // ��Ÿ�� ���� �ð� 

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
    public bool IsCoolTime => _isCoolTime;
    public float RemainTime => _remainTime; 

    public bool IsDelayed => throw new NotImplementedException();

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
    public virtual bool Attack() 
    {
        if (_isCoolTime == true) return false; // ��Ÿ�����̸� ���� 

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
        if(attackInfo.attackSO.isRayAttack == true)
        {
            FindTarget(); // Ÿ�� ã�� ���� �ǵ�� ���� 
        }
        else
        {
            attackCollider.ActiveCollider(true, true);
        }

        // �ǵ�� ���� 
        if (_fov.TargetList.Count >= 1)
        {
            attackInfo.feedbackCallbackHit?.Invoke(); 
        }
        // ��? 
        _agentAnimation.Update_Zero();
        attackInfo.feedbackCallback?.Invoke();

        // ��Ÿ�� �ֱ� 
        CoolTime();
        return true;  // ���������� ���� �� 
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

    // ��ų ����� ��Ÿ�� �ֱ� 
    public void CoolTime()
    {
        _isCoolTime = true; 
        _remainTime = attackInfo.attackSO.attackCoolTime;


        GameManager.Instance.CoroutineComponent.SetCoroutine(CheckCoolTime());
        GameManager.Instance.BegineCoroutine(); 
    }

    IEnumerator CheckCoolTime()
    {
        while(true)
        {
            _remainTime -= Time.deltaTime;
            if (_remainTime <= 0)
            {
                _remainTime = 0;
                _isCoolTime = false;
                break; 
            }
            yield return null; 
        }
    }
}
