﻿using UnityEngine;
using System;
using System.Collections.Generic;

public enum AttackType
{
    Null,
    Default_1 = 100,
    Default_2,
    Default_3,
 
    Tackle = 1000, 
}


public class AttackModule : MonoBehaviour, IComponent
{
    // 캐싱 변수 
    private IAgent owner; 
    private FieldOfView _fov;
    private PlayerAnimation _agentAnimation;
    private PlayerMoveModule _moveModule;
    private TimerModule _timerModule; 
        

    [SerializeField]
    private bool isEnemy; 
    [SerializeField]
    private List<AttackBase> attackInfoList = new List<AttackBase>();

    [SerializeField]
    private AttackBase _curAttackBase;
    [SerializeField]
    private LayerMask _hitLayerMask;

    // 프로퍼티
    public AttackSO CurAttackSO => _curAttackBase.attackInfo.attackSO;
    public AttackType CurAttackType => _curAttackBase.attackInfo.attackType;
    public AttackType NextAttackType => _curAttackBase.attackInfo.nextAttackType;


    private void Start()
    {
        _hitLayerMask =  (isEnemy) ? 1 << LayerMask.NameToLayer("Player") : 1 << LayerMask.NameToLayer("Enemy");
        _curAttackBase = attackInfoList[0]; 
    }

    public void Init(IAgent owner, FieldOfView fov,PlayerAnimation agentAnimation)
    {
        this.owner = owner; 
        _fov = fov;
        _agentAnimation = agentAnimation;

        InitAttackinfo(); 
    }

    public void DefaultAttack()
    {
        //if(CheckAttack() == true)
            Debug.Log(_curAttackBase.attackInfo.attackSO.animationFuncName);
            if(_curAttackBase.Attack() == false && isEnemy == false) // 쿨타임 중이면서 플레이어면 
            {
                // 커서에 쿨타임 표시 
                CursorCoolTimeUI coolTimeText = PoolManager.Instance.Pop("CursorCoolTimeUI") as CursorCoolTimeUI;
                coolTimeText.UpdateCoolTimeText(_curAttackBase.RemainTime);    
            }
    }

    /// <summary>
    /// 실질적인 공격 수행 ( 애니메이션 중간 부분에 Event로 추가 ) 
    /// </summary>
    public void AttackJudge() 
    {
        Debug.Log("공격 판단");
        _curAttackBase.AttackJudge(); 
    }

    public void SetCurAttackType(AttackType attackType)
    {
        attackInfoList.ForEach((x) =>
        {
            if (x.attackInfo.attackType == attackType)
            {
                _curAttackBase = x;
                return;
            }
        });
    }
    
    private void InitAttackinfo()
    {
        foreach(var info in attackInfoList)
        {
            info.Init(owner, _agentAnimation, _moveModule, _fov);
        }
    }

    public void ActiveFalseCollider()
    {
        // 콜라이더로 공격 판정한다면 
        if(_curAttackBase.attackCollider != null) 
        {
            _curAttackBase.attackCollider.ActiveCollider(false);// 꺼주기 
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (_curAttackInfo == null) _curAttackInfo = attackInfoList[0]; 
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawSphere(transform.position, CurAttackSO.attackRadius);

    //}
}
