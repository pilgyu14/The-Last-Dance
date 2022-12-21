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
    //public AttackType attackType;
    public AttackType nextAttackType;
    public AttackSO attackSO;
    public UnityEvent feedbackCallback = null; // 기본적인 피드백 ( 스윙할 때 사운드 ) , 스킬 공격할 때 다른 행동( 태클일 경우 -> 슬라이딩 움직임 ) 
    public UnityEvent feedbackCallbackHit = null; // 때렸을 때 나올 피드백 ( 카메라 흔들림 ) 
}


[System.Serializable]
public class AttackBase : ICoolTime
{
    // 상태 변수 
    private bool _isCoolTime = false; // 쿨타임중인가 
    private float _remainTime;  // 쿨타임 남은 시간 

    private Action AttackEvent = null;
    // 캐싱 변수 
    private IAgent _owner;

    private PlayerAnimation _playerAnimation = null;
    private EnemyAnimation _enemyAnimation = null;
    // 적의 경우 생각
    // attackSO.IsEnemy 있다 

    private AttackJudgementComponent _atkJudgeComponent;
    private AgentMoveModule _moveModule;
    private FieldOfView _fov;

    // 인스펙터 참조 변수 
    public AttackInfo attackInfo;
    public AttackCollider attackCollider;

    // 프로퍼티 
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
    /// 실제적인 공격  
    /// </summary>
    public virtual bool Attack() 
    {
        if (_isCoolTime == true) return false; // 쿨타임중이면 리턴 

        Debug.Log("공격!");
        // 움직임 멈추고
        _moveModule.StopMove();
        
        // 애니메이션 실행 
        if(attackInfo.attackSO.animationFuncName != "")
        {
            Debug.Log("공격 애니메이션 수행");

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
            Debug.Log("공격 애니메이션 수행");
            _enemyAnimation.ChangeAttackAnimation(attackInfo.attackSO.animationClip); //공격 애니메이션 변경 
           _enemyAnimation.PlayAttack(); 
            // 공격 정보에 어택 클립 받고 
            // 컨트롤러에 넣고 
            // 실행 
        }
        _owner.AudioPlayer.PlayClip(attackInfo.attackSO.swingClip);

        // 피드백 실행 
        //if (_fov.TargetList.Count >= 1)
        //{
        //    attackInfo.feedbackCallbackHit?.Invoke(); 
        //}
        // 음? 

        if (_playerAnimation != null)
        {
            _playerAnimation.Update_Zero();
        }
        else
        {
            _enemyAnimation.Update_Zero();
        }
        attackInfo.feedbackCallback?.Invoke();

        // 쿨타임 주기 
        CoolTime();
        return true;  // 성공적으로 공격 끝 
    }

    /// <summary>
    /// 실질적인 공격 수행 ( 애니메이션 중간에 추가 )     
    /// </summary>
    public void AttackJudge()
    {
        AttackEvent?.Invoke(); 
    }

    // 레이캐스트 공격
    private void RayAttack()
    {
        Debug.Log("레이 공격");
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // 범위 안 몬스터 찾기 

        foreach (var target in _fov.TargetList) // 타겟 공격 판정( 데미지 , 넉백 )  
        {
            // 여기를 잘 처리해야해
            _atkJudgeComponent.AttackJudge(target);
        }
    }

    // 콜라이더 공격 
    private void ColliderAttack()
    {
        Debug.Log("콜라이더 공격");
        attackCollider.ActiveCollider(true, attackInfo.attackSO.isContinueColliderAttack); // 이거 늦게 
    }

    public void ActiveEffect()
    {

    }

    // 스킬 사용후 쿨타임 주기 
    public void CoolTime()
    {
        if (attackInfo.attackSO.attackCoolTime <= 0) return; 
        _isCoolTime = true; 
        _remainTime = attackInfo.attackSO.attackCoolTime;


        GameManager.Instance.CoroutineComponent.SetCoroutine(CheckCoolTime());
        GameManager.Instance.BegineCoroutine(); 
    }

    /// <summary>
    /// 지속 공격이 끝났는가 
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
