using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rito.BehaviorTree;

using static Rito.BehaviorTree.NodeHelper;
using System;


// 몬스터마다 이거 상속받고 
// owner 변수 T로  새로 만들고 _owner T로 캐싱해서 받기 
// MeleeEnemy owner = _owner as MeleeEnemy; 

public class EnemyTree<T> : ICore where T : Enemy
{

    protected T _owner;
    protected INode _rootNode;
    protected Transform _target;

    protected FieldOfView _fov;
    public EnemyTree(T owner)
    {
        this._owner = owner;
        SetNode();
        SetComponents();
    }
    public void UpdateRun()
    {
        _rootNode.Run();
    }

    /// <summary>
    /// 컴포넌트들 캐싱
    /// </summary>
    protected void SetComponents()
    {
        _fov = _owner.EnemyComponents[typeof(FieldOfView)] as FieldOfView;
    }
    // 적마다 달라질 부분 
    protected virtual void SetNode()
    {
        _rootNode =

            // true 나올때까지 순회 
            Selector
            (
                Condition(_owner.IsDie), // 죽었냐
                Condition(_owner.IsHit), // 맞고있냐 

                // false 나올때까지 순회  
                Sequence // 공격 시퀀스 
                (

                    // 쿨타임 중이면 리턴 
                    Condition(_owner.CheckAttack), // 공격범위 안에 있냐 
                    Action(_owner.DefaultAttack) // 여기까지 왔으면 공격해라 

            //IfAction(), 

            //   IfAction(CheckDistance,),
            /*
             * 공격 패턴 
             * - 랜덤 실행 노드로 하나 골라서 실행 
             * - 공격마다 범위 다름 -> 범위 어려운 것부터 체크 후 
             *   true 반환하는 거 있으면 그거 실행 
             * 
             */
                ),
                
                Sequence // 추적 시퀀스 
                (
                    
                    // 타겟이 추적 범위 내에 있는지 체크
                    Condition(_owner.CheckChase) 
                ),

                Sequence
                (
                    
                )
            ); 
        
    }

    /// <summary>
    /// 현재 상태에 따른 노드 설정 
    /// 난이도 or 체력이 별로 없을 떄 다르게 처리
    /// </summary>
    protected virtual void SetNodeByState()
    {

    }

}
