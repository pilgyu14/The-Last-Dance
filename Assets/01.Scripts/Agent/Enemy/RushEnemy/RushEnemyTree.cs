using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.BehaviorTree;

using static Rito.BehaviorTree.NodeHelper;

public class RushEnemyTree : EnemyTree<RushEnemy>
{

    public RushEnemyTree(RushEnemy owner) : base(owner)
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
        Sequence // 기본 공격 시퀀스 
        (


            NotCondition(_owner.CheckDefaultAttackCoolTime), // 공격이 쿨타임이냐 (그러면서 적이 범위 안에 있으면 기다려 
            Condition(_owner.CheckMeleeAttack), // 공격범위 안에 있냐 
            Action(_owner.DefaultAttack_1) // 여기까지 왔으면 공격해라 

        //IfAction(), 

        //   IfAction(CheckDistance,),
        /*
         * 공격 패턴 
        *    먼저 스킬부터 체크

         * - 랜덤 실행 노드로 하나 골라서 실행 
         * - 공격마다 범위 다름 -> 범위 어려운 것부터 체크 후 ** 고려해볼 방안 
         *   true 반환하는 거 있으면 그거 실행 
         *   
         *    쿨타임이냐 
         *    범위 안에 있냐 체크
         * ---------------------------

         *    기본 공격 체크   
         *    공격중이냐
         *    범위 안에 잇냐
         */
        ),

        // 돌진 공격 시퀀스 
        Sequence 
        (
            NotCondition(_owner.CheckRushCoolTime),
            Condition(_owner.CheckRushAttack),
            Action(_owner.RushAttack)
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
           // Condition(_owner.CheckChase),
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
