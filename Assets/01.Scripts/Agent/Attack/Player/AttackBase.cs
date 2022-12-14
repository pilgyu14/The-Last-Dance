using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FeedbackInfo
{
    // 카메라 흔들림 피드백
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
    public UnityEvent feedbackCallback = null; // 기본적인 피드백 ( 스윙할 때 사운드 ) , 스킬 공격할 때 다른 행동( 태클일 경우 -> 슬라이딩 움직임 ) 
    public UnityEvent feedbackCallbackHit = null; // 때렸을 때 나올 피드백 ( 카메라 흔들림 ) 
}


[System.Serializable]
public class AttackBase 
{
    // 캐싱 변수 
    private IAgent _owner; 

    private PlayerAnimation _agentAnimation;
    // 적의 경우 생각
    // attackSO.IsEnemy 있다 

    private AttackJudgementComponent _atkJudgeComponent; 
    private PlayerMoveModule _moveModule;
    private FieldOfView _fov;

    // 인스펙터 참조 변수 
    public AttackInfo attackInfo;
    public AttackCollider attackCollider;

    // 프로퍼티 
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
    /// 실제적인 공격 판정 
    /// </summary>
    public virtual void Attack() 
    {
        // 움직임 멈추고
        _moveModule.StopMove();
        
        // 애니메이션 실행 
        if(attackInfo.attackSO.animationFuncName != null)
        {
            Type type = typeof(PlayerAnimation);
            MethodInfo method = type.GetMethod(attackInfo.attackSO.animationFuncName);
            method?.Invoke(_agentAnimation, new object[] { });
        }

        // 공격 수행 
        ////
        if(attackInfo.attackSO.isRayAttack == true)
        {
            FindTarget(); // 타겟 찾고 공격 피드백 수행 
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
        // 음? 
        _agentAnimation.Update_Zero();

        attackInfo.feedbackCallback?.Invoke(); 
    }

    private void FindTarget()
    {
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // 범위 안 몬스터 찾기 

        foreach (var target in _fov.TargetList) // 타겟 공격 판정( 데미지 , 넉백 )  
        {
            // 여기를 잘 처리해야해
            _atkJudgeComponent.AttackJudge(target);
        }
    }

    public void ActiveEffect()
    {

    }
    
}

