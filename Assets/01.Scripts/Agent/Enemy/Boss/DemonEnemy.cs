using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonEnemy : Enemy
{
    private DemonEnemyTree _demonEnemyTree; 
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        _demonEnemyTree.UpdateRun(); 
    }

    protected override void CreateTree()
    {
        _demonEnemyTree = new DemonEnemyTree(this);
    }

    #region Condition

    // 찌르기와 기본 공격 범위는 같다. 
    public bool CheckStingCoolTime()
    {
        Debug.Log("찌르기 쿨타임 체크");
        return _attackModule.GetAttackInfo(AttackType.StingAttack).IsCoolTime;
    }



    // @@ 스킬 @@ // 

    public bool CheckChargeCutTime()
    {
        Debug.Log("차징 베기 쿨타임 체크");
        return _attackModule.GetAttackInfo(AttackType.CutAttack).IsCoolTime;
    }
    /// <summary>
    /// 차징 베기 공격 범위 체크 
    /// </summary>
    /// <returns></returns>
    public bool CheckChargeAttack()
    {
        Debug.Log("차징 베기 범위 체크");
        float atkDistance = _attackModule.GetAttackInfo(AttackType.CurChargeAttack).attackInfo.attackSO.attackRadius;
        return CheckDistance(_enemySO.eyeAngle, atkDistance);
    }

    public bool CheckJumpAttackTime()
    {
        Debug.Log("점프 공격 쿨타임 체크");
        return _attackModule.GetAttackInfo(AttackType.CutAttack).IsCoolTime;
    }
    /// <summary>
    /// 차징 베기 공격 범위 체크 
    /// </summary>
    /// <returns></returns>
    public bool CheckJumpAttack()
    {
        Debug.Log("점프 공격 범위 체크");
        float atkDistance = _attackModule.GetAttackInfo(AttackType.CurChargeAttack).attackInfo.attackSO.attackRadius;
        return CheckDistance(_enemySO.eyeAngle, atkDistance);
    }


    #endregion

    #region Action

    /// <summary>
    /// 찌르기 공격 
    /// </summary>
    public void StingAttack()
    {

    }

    /// <summary>
    /// 적 소환 
    /// </summary>
    public void SummonEnemy()
    {

    }

    /// <summary>
    /// 점프 공격
    /// </summary>
    public void JumpAttack()
    {

    }

    /// <summary>
    /// 차징 베기 
    /// </summary>
    public void ChargeCutAttack()
    {
        
    }


    #endregion 


}
