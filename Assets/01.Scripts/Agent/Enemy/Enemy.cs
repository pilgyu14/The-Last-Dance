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

    private Dictionary<Type, IComponent> _enemyComponents = new Dictionary<Type, IComponent>();  // ��� ������Ʈ ���� ��ųʸ� 

    private HPModule _hpModule; 
    // ĳ�� ���� 
    private AgentMoveModule<Enemy> _moveModule;
    private AttackModule _attackModule;
    private FieldOfView _fov;
    private NavMeshAgent _agent;
    private EnemyAnimation _enemyAnimation;

    // ������Ƽ 
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
    // ������Ƽ 
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

    // �ǰ� ���� 
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
