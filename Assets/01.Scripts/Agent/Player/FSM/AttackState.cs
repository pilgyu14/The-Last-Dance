using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<PlayerController>
{
    TimerModule timer;
    private AttackType _curAttackType;
    private AttackType _nextAttackType;

    public AttackType NextAttackType => _nextAttackType;
    public AttackType CurAttackType => _curAttackType;
    public override void Enter()
    {
        owner.StartAttack();
        owner.InputModule.OnMovementKeyPress += owner.MoveModule.InBattleMove;
        _nextAttackType = owner.AttackModule.NextAttackType;
        _curAttackType = owner.AttackModule.CurAttackType;
        timer = new TimerModule(1f, () => owner.EndAttackState());
        // 시간 카운트 
        // 공격 
    }

    public override void Stay()
    {
        timer.UpdateSomething();
    }

    public override void Exit()
    {
        owner.InputModule.OnMovementKeyPress -= owner.MoveModule.InBattleMove;
        timer = null;
    }
}