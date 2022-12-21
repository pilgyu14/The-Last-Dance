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

    // true ���ö����� ��ȸ 
    Selector
    (
        Condition(_owner.IsDie), // �׾���
        Condition(_owner.IsHit), // �°��ֳ� 
        Condition(_owner.IsStunned), // �������̳� 
        Condition(_owner.IsAttacking), // �̹� ������ ���̳� 

        // false ���ö����� ��ȸ  
        // �⺻ ���� ������  
        Sequence
        (
            Condition(_owner.CheckMeleeAttack), // ���ݹ��� �ȿ� �ֳ� 
            Random
            (
                Sequence // ���� ���� 
                (
                    NotCondition(_owner.CheckDefaultAttackCoolTime), // ������ ��Ÿ���̳� (�׷��鼭 ���� ���� �ȿ� ������ ��ٷ� 
                    Action(_owner.DefaultAttack_1) // ������� ������ �����ض� 
                ),
                Sequence // ��� 
                (
                    NotCondition(_owner.CheckStingCoolTime), // ������ ��Ÿ���̳� (�׷��鼭 ���� ���� �ȿ� ������ ��ٷ� 
                    Action(_owner.StingAttack) // ������� ������ �����ض� 
                )
            )
        ),

        // ���� ���� ������ 
        Sequence
        (
            NotCondition(_owner.CheckChargeCutTime)//,

            //Condition(_owner.CheckRushAttack),
            //Action(_owner.RushAttack)
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
            Condition(_owner.CheckChase),
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
