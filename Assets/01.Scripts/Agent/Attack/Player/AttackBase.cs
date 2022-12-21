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
    //public AttackType attackType;
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

    private Action AttackEvent = null;
    // ĳ�� ���� 
    private IAgent _owner;

    private PlayerAnimation _playerAnimation = null;
    private EnemyAnimation _enemyAnimation = null;
    // ���� ��� ����
    // attackSO.IsEnemy �ִ� 

    private AttackJudgementComponent _atkJudgeComponent;
    private AgentMoveModule _moveModule;
    private FieldOfView _fov;

    // �ν����� ���� ���� 
    public AttackInfo attackInfo;
    public AttackCollider attackCollider;

    // ������Ƽ 
    private bool IsEnemyAtk => attackInfo.attackSO.isEnemy;
    private float AttackDuration => attackInfo.attackSO.attackDuration;
    public AttackJudgementComponent AtkJudgeComponent => _atkJudgeComponent;
    public bool IsCoolTime => _isCoolTime;
    public float RemainTime => _remainTime;

    public Type OwnerType
    {
        get
        {
            Type type;
            if (IsEnemyAtk == true)
                type = typeof(Enemy);
            else
                type = typeof(PlayerController);
            return type; 
        }
    }


public void Init(IAgent owner, AgentAnimation playerAnimation,AgentMoveModule playerMoveModule ,FieldOfView fov)
    {
        _owner = owner; 
        if(IsEnemyAtk == true)
        {
            _enemyAnimation = playerAnimation as EnemyAnimation;
        }
        else
        {
            _playerAnimation = playerAnimation as PlayerAnimation;

        }
        _moveModule = playerMoveModule; 
        _fov = fov;

        _atkJudgeComponent = new AttackJudgementComponent();
        _atkJudgeComponent.Init(owner, attackInfo);

        if(attackCollider != null)
        {
            attackCollider.Init(this);
        }
        if (attackInfo.attackSO.isRayAttack == true)
        {
            AttackEvent = RayAttack;
        }
        else
        {
            AttackEvent = ColliderAttack;
        }
    }

    /// <summary>
    /// �������� ����  
    /// </summary>
    public virtual bool Attack() 
    {
        if (_isCoolTime == true) return false; // ��Ÿ�����̸� ���� 

        Debug.Log("����!");
        // ������ ���߰�
        _moveModule.StopMove();
        
        // �ִϸ��̼� ���� 
        if(attackInfo.attackSO.animationFuncName != "")
        {
            Debug.Log("���� �ִϸ��̼� ����");

            Type type = typeof(PlayerAnimation);
            MethodInfo method = type.GetMethod(attackInfo.attackSO.animationFuncName);
            ParameterInfo[] parameterInfo = method.GetParameters();
            if(parameterInfo.Length > 0 && parameterInfo[0].ParameterType.Name == "Boolean")
            {
                method?.Invoke(_playerAnimation, new object[] { true});
            }
            else
            {
                method?.Invoke(_playerAnimation, new object[] { });
            }
        }
        else if(attackInfo.attackSO.animationClip != null)
        {
            Debug.Log("���� �ִϸ��̼� ����");
            _enemyAnimation.ChangeAttackAnimation(attackInfo.attackSO.animationClip); //���� �ִϸ��̼� ���� 
           _enemyAnimation.PlayAttack(); 
            // ���� ������ ���� Ŭ�� �ް� 
            // ��Ʈ�ѷ��� �ְ� 
            // ���� 
        }
        _owner.AudioPlayer.PlayClip(attackInfo.attackSO.swingClip);

        // �ǵ�� ���� 
        //if (_fov.TargetList.Count >= 1)
        //{
        //    attackInfo.feedbackCallbackHit?.Invoke(); 
        //}
        // ��? 

        if (_playerAnimation != null)
        {
            _playerAnimation.Update_Zero();
        }
        else
        {
            _enemyAnimation.Update_Zero();
        }
        attackInfo.feedbackCallback?.Invoke();

        // ��Ÿ�� �ֱ� 
        CoolTime();
        return true;  // ���������� ���� �� 
    }

    /// <summary>
    /// �������� ���� ���� ( �ִϸ��̼� �߰��� �߰� )     
    /// </summary>
    public void AttackJudge()
    {
        AttackEvent?.Invoke(); 
    }

    // ����ĳ��Ʈ ����
    private void RayAttack()
    {
        Debug.Log("���� ����");
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // ���� �� ���� ã�� 

        foreach (var target in _fov.TargetList) // Ÿ�� ���� ����( ������ , �˹� )  
        {
            // ���⸦ �� ó���ؾ���
            _atkJudgeComponent.AttackJudge(target);
        }
    }

    // �ݶ��̴� ���� 
    private void ColliderAttack()
    {
        Debug.Log("�ݶ��̴� ����");
        attackCollider.ActiveCollider(true, attackInfo.attackSO.isContinueColliderAttack); // �̰� �ʰ� 
    }

    public void ActiveEffect()
    {

    }

    // ��ų ����� ��Ÿ�� �ֱ� 
    public void CoolTime()
    {
        if (attackInfo.attackSO.attackCoolTime <= 0) return; 
        _isCoolTime = true; 
        _remainTime = attackInfo.attackSO.attackCoolTime;


        GameManager.Instance.CoroutineComponent.SetCoroutine(CheckCoolTime());
        GameManager.Instance.BegineCoroutine(); 
    }

    /// <summary>
    /// ���� ������ �����°� 
    /// </summary>
    /// <returns></returns>
    public bool lsEndAttackDuration()
    {
        bool isEnd = attackInfo.attackSO.attackCoolTime - _remainTime  >= AttackDuration;
        return isEnd; 
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
