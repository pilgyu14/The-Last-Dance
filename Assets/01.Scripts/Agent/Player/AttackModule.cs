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
    // 캐싱 변수 
    private FieldOfView _fov;
    private PlayerAnimation _agentAnimation;
    private MoveModule _moveModule;
    private TimerModule _timerModule; 
        

    [SerializeField]
    private bool isEnemy; 
    [SerializeField]
    private List<AttackBase> attackInfoList = new List<AttackBase>();

    private AttackBase _curAttackInfo;
    [SerializeField]
    private LayerMask _hitLayerMask;

    // 프로퍼티
    public AttackSO curAttackSO => _curAttackInfo.attackInfo.attackSO;
    public AttackType curAttackType => _curAttackInfo.attackInfo.attackType; 



    private void Start()
    {
        _hitLayerMask =  (isEnemy) ? 1 << LayerMask.NameToLayer("Player") : 1 << LayerMask.NameToLayer("Enemy");
        _curAttackInfo = attackInfoList[0]; 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            // 입력 키에 따라 어떤 공격할지 설정 
            DefaultAttack(); 
        }
    }
    public void Init(FieldOfView fov,MoveModule moveModule ,PlayerAnimation agentAnimation)
    {
        _fov = fov;
        _moveModule = moveModule;
        _agentAnimation = agentAnimation;

        InitAttackinfo(); 
    }

    public void DefaultAttack()
    {
        if(CheckAttack() == true)
        {
            Debug.Log(_curAttackInfo.attackInfo.attackSO.animationFuncName);
            _curAttackInfo.Attack();
        }
    }

    private bool CheckAttack()
    {
        if (_agentAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _agentAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || _agentAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackMove"))
        {
            SetCurAttackType(AttackType.Default_1);
        }
        else if (_agentAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).IsName("front_kick") && _agentAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
        {
            SetCurAttackType(AttackType.Default_2);
        }
        else if (_agentAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).IsName("SideKick") && _agentAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
        {
            SetCurAttackType(AttackType.Default_3);
        }
        else
        {
            Debug.Log("공격 데이터 없어요");
            return false; 
        }
        return true; 
    }

    private float _nextAtkTImeLimit = 0.2f; 
    public void CheckAttackType()
    {
        //if(_curAttackInfo.attackType == AttackType.Default_1)
        //{
        //    _moveModule.StopMove(); 
        //    _agentAnimation.Update_Zero(); 
        //}

    }

    public void SetCurAttackType(AttackType attackType)
    {
        attackInfoList.ForEach((x) =>
        {
            if (x.attackInfo.attackType == attackType)
            {
                _curAttackInfo = x;
                return;
            }
        });
    }
    
    private void InitAttackinfo()
    {
        foreach(var info in attackInfoList)
        {
            info.Init(gameObject, _agentAnimation, _moveModule, _fov);
        }
    }

    private void OnDrawGizmos()
    {
        if (_curAttackInfo == null) _curAttackInfo = attackInfoList[0]; 
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, curAttackSO.attackRadius);

    }
}
