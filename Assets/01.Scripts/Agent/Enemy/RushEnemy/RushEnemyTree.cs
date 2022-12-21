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

    // true ���ö����� ��ȸ 
    Selector
    (
        Condition(_owner.IsDie), // �׾���
        Condition(_owner.IsHit), // �°��ֳ� 
        Condition(_owner.IsStunned), // �������̳� 
        Condition(_owner.IsAttacking), // �̹� ������ ���̳� 

        // false ���ö����� ��ȸ  
        Sequence // �⺻ ���� ������ 
        (


            NotCondition(_owner.CheckDefaultAttackCoolTime), // ������ ��Ÿ���̳� (�׷��鼭 ���� ���� �ȿ� ������ ��ٷ� 
            Condition(_owner.CheckMeleeAttack), // ���ݹ��� �ȿ� �ֳ� 
            Action(_owner.DefaultAttack_1) // ������� ������ �����ض� 

        //IfAction(), 

        //   IfAction(CheckDistance,),
        /*
         * ���� ���� 
        *    ���� ��ų���� üũ

         * - ���� ���� ���� �ϳ� ��� ���� 
         * - ���ݸ��� ���� �ٸ� -> ���� ����� �ͺ��� üũ �� ** ����غ� ��� 
         *   true ��ȯ�ϴ� �� ������ �װ� ���� 
         *   
         *    ��Ÿ���̳� 
         *    ���� �ȿ� �ֳ� üũ
         * ---------------------------

         *    �⺻ ���� üũ   
         *    �������̳�
         *    ���� �ȿ� �ճ�
         */
        ),

        // ���� ���� ������ 
        Sequence 
        (
            NotCondition(_owner.CheckRushCoolTime),
            Condition(_owner.CheckRushAttack),
            Action(_owner.RushAttack)
        ),
        // �¾��� �� Ÿ�� �ٶ󺸱� 

        // ���� ������ 
        Sequence
        (
            Parallel
            (

            ),

            // Ÿ���� �� ���ȳ�  
            // Ÿ�� �ٶ󺸱�
            // Ÿ���� ���� ���� ���� �ִ��� üũ
           // Condition(_owner.CheckChase),
            Action(_owner.Chase)

        // ���� ���� 
        ),

        Selector // �⺻ ���� ������ 
        (
            IfNotAction(_owner.IsOriginPos, _owner.MoveOrigin), // �⺻ ��ġ�� ������ �������ݾ� 
            Action(_owner.Idle)
        // ���� ����. 
        )
    );
    }
}
