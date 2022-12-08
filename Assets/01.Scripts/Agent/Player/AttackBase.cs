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
    public UnityEvent feedbackCallback = null; // 기본적인 피드백 ( 스윙할 때 사운드 ) 
    public UnityEvent feedbackCallbackHit = null; // 때렸을 때 나올 피드백 ( 카메라 흔들림 ) 
}


[System.Serializable]
public class AttackBase 
{
    // 캐싱 변수 
    private GameObject _owner; 

    private PlayerAnimation _agentAnimation;
    // 적의 경우 생각
    // attackSO.IsEnemy 있다 

    private PlayerMoveModule _moveModule;
    private FieldOfView _fov;

    // 인스펙터 참조 변수 
    public AttackInfo attackInfo;

    public void Init(GameObject owner, PlayerAnimation playerAnimation, PlayerMoveModule moveModule,FieldOfView fov)
    {
        _owner = owner; 
        _agentAnimation = playerAnimation;
        _moveModule = moveModule;
        _fov = fov; 
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

        // 공격 판정 
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // 범위 안 몬스터 찾기 

        foreach (var target in _fov.TargetList) // 타겟 공격 판정( 데미지 , 넉백 )  
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable.GetDamaged(attackInfo.attackSO.attackDamage, _owner);

            // 이펙트 
            Vector3 hitPos = target.position;
            // 맞은 위치 
            // 히트 오디오 재생 

            // 플레이어 공격이라면 
            // 데미지 텍스트 
            DamageText damageText = new DamageText(); // 풀링으로 
            damageText.SetText(attackInfo.attackSO.attackDamage, _owner.transform.position + new Vector3(0, 0.5f, 0), Color.white, false);
            
            // 흔들림 

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
        // 음? 
        _agentAnimation.Update_Zero();

        attackInfo.feedbackCallback?.Invoke(); 
    }

    
}

