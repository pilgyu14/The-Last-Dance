using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rito.BehaviorTree;

using static Rito.BehaviorTree.NodeHelper;
using System;

public class Enemy : MonoBehaviour,IDamagable,IAgent, IAgentInput
{
    [SerializeField]
    private AgentSO _enemySO;

    private Dictionary<Type, IComponent> _enemyComponents = new Dictionary<Type, IComponent>();  // 모든 컴포넌트 저장 딕셔너리 

    private HPModule _hpModule; 
    // 캐싱 변수 
    private AgentMoveModule<Enemy> _moveModule;
    private AttackModule _attackModule;
    private FieldOfView _fov;
    private NavMeshAgent _agent;
    private EnemyAnimation _enemyAnimation;

    // 프로퍼티 
    public Dictionary<Type, IComponent> EnemyComponents => _enemyComponents;
    private void Awake()
    {
        _moveModule = GetComponent<AgentMoveModule<Enemy>>();
        _attackModule = GetComponent<AttackModule>();
        _fov = GetComponent<FieldOfView>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _hpModule = GetComponent<HPModule>(); 
    }

    private void Start()
    {
        SetComponents(); 
    }

    private Vector3 _hitPoint; 
    // 프로퍼티 
    public bool IsEnemy => true;

    public Vector3 HitPoint => _hitPoint;

    public Action<Vector3> OnMovementKeyPress { get;  set; }
    public Action<Vector3> OnPointerRotate { get; set; }
    public Action OnDefaultAttackPress { get; set; }


    private void SetComponents()
    {
        _enemyComponents.Add(typeof(AgentMoveModule<Enemy>), _moveModule);
        _enemyComponents.Add(typeof(AttackModule), _attackModule);
        _enemyComponents.Add(typeof(FieldOfView), _fov);
        _enemyComponents.Add(typeof(EnemyAnimation), _enemyAnimation);
    }

    public void CheckDistance()
    {

    }

    // 피격 관련 
    public void GetDamaged(int damage, GameObject damageDealer)
    {
    }

    public bool IsDie()
    {
        return false; 
    }

    public void OnDie()
    {
    }

}
