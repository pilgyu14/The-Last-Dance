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

    // ���� �⺻ ���� ������ ����. 
    public bool CheckStingCoolTime()
    {
        Debug.Log("��� ��Ÿ�� üũ");
        return _attackModule.GetAttackInfo(AttackType.StingAttack).IsCoolTime;
    }



    // @@ ��ų @@ // 

    public bool CheckChargeCutTime()
    {
        Debug.Log("��¡ ���� ��Ÿ�� üũ");
        return _attackModule.GetAttackInfo(AttackType.CutAttack).IsCoolTime;
    }
    /// <summary>
    /// ��¡ ���� ���� ���� üũ 
    /// </summary>
    /// <returns></returns>
    public bool CheckChargeAttack()
    {
        Debug.Log("��¡ ���� ���� üũ");
        float atkDistance = _attackModule.GetAttackInfo(AttackType.CurChargeAttack).attackInfo.attackSO.attackRadius;
        return CheckDistance(_enemySO.eyeAngle, atkDistance);
    }

    public bool CheckJumpAttackTime()
    {
        Debug.Log("���� ���� ��Ÿ�� üũ");
        return _attackModule.GetAttackInfo(AttackType.CutAttack).IsCoolTime;
    }
    /// <summary>
    /// ��¡ ���� ���� ���� üũ 
    /// </summary>
    /// <returns></returns>
    public bool CheckJumpAttack()
    {
        Debug.Log("���� ���� ���� üũ");
        float atkDistance = _attackModule.GetAttackInfo(AttackType.CurChargeAttack).attackInfo.attackSO.attackRadius;
        return CheckDistance(_enemySO.eyeAngle, atkDistance);
    }


    #endregion

    #region Action

    /// <summary>
    /// ��� ���� 
    /// </summary>
    public void StingAttack()
    {

    }

    /// <summary>
    /// �� ��ȯ 
    /// </summary>
    public void SummonEnemy()
    {

    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void JumpAttack()
    {

    }

    /// <summary>
    /// ��¡ ���� 
    /// </summary>
    public void ChargeCutAttack()
    {
        
    }


    #endregion 


}
