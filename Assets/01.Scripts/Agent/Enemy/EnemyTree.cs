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

public class EnemyTree<T> : MonoBehaviour, ICore where T : Enemy
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
    private void Update()
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

            Selector
            (
             //   IfAction(CheckDistance,),
            );
    }
    
    public bool CheckDistance()
    {
       // _fov.FindTargets
        //    if(_target.position - transform.position < )
        return false;
    }
}
