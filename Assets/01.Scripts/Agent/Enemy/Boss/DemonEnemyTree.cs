using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.BehaviorTree;

using static Rito.BehaviorTree.NodeHelper;
public class DemonEnemyTree : EnemyTree<DemonEnemy>
{
    public DemonEnemyTree(DemonEnemy owner) : base(owner)
    {

    }

    protected override void SetNode()
    {
        _rootNode =

    // true 나올때까지 순회 
    Selector
    (
        Condition(_owner.IsDie), // 죽었냐
        Condition(_owner.IsHit), // 맞고있냐 
        Condition(_owner.IsStunned), // 기절중이냐 
        Condition(_owner.IsAttacking), // 이미 때리는 중이냐 

        // false 나올때까지 순회  
        // 기본 공격 시퀀스  
        Sequence
        (
            Condition(_owner.CheckMeleeAttack), // 공격범위 안에 있냐 
            Random
            (
                Sequence // 가로 베기 
                (
                    NotCondition(_owner.CheckDefaultAttackCoolTime), // 공격이 쿨타임이냐 (그러면서 적이 범위 안에 있으면 기다려 
                    Action(_owner.DefaultAttack_1) // 여기까지 왔으면 공격해라 
                ),
                Sequence // 찌르기 
                (
                    NotCondition(_owner.CheckStingCoolTime), // 공격이 쿨타임이냐 (그러면서 적이 범위 안에 있으면 기다려 
                    Action(_owner.StingAttack) // 여기까지 왔으면 공격해라 
                )
            )
        ),

        // 돌진 공격 시퀀스 
        Sequence
        (
            NotCondition(_owner.CheckChargeCutTime)//,

            //Condition(_owner.CheckRushAttack),
            //Action(_owner.RushAttack)
        ),
        // 맞았을 때 타겟 바라보기 

        // 추적 시퀀스 
        Sequence
        (
            Parallel
            (

            ),

            // 타겟이 날 때렸냐  
            // 타겟 바라보기
            // 타겟이 추적 범위 내에 있는지 체크
            Condition(_owner.CheckChase),
            Action(_owner.Chase)

        // 추적 시작 
        ),

        Selector // 기본 상태 시퀀스 
        (
            IfNotAction(_owner.IsOriginPos, _owner.MoveOrigin), // 기본 위치에 있으면 움직이잖아 
            Action(_owner.Idle)
        // 빙빙 돈다. 
        )
    );
    }


}
