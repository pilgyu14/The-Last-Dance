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
    private EnemySO _enemySO;
    private Transform _target;
    private EnemyTree<Enemy> _enemyTree; 

    private Dictionary<Type, IComponent> _enemyComponents = new Dictionary<Type, IComponent>();  // 모든 컴포넌트 저장 딕셔너리 

    private HPModule _hpModule; 
    // 캐싱 변수 
    private AgentMoveModule<Enemy> _moveModule;
    private AttackModule _attackModule;
    private FieldOfView _fov;
    private NavMeshAgent _agent;
    private EnemyAnimation _enemyAnimation;

    // 상태 변수 
    private bool _isHit = false; // 피격중인가
    private bool _isBattleMode = false; // 전투중인가 

    private Vector3 _hitPoint;
    // 프로퍼티 
    public bool IsEnemy => true;
    public Vector3 HitPoint => _hitPoint;

    public Action<Vector3> OnMovementKeyPress { get; set; }
    public Action<Vector3> OnPointerRotate { get; set; }
    public Action OnDefaultAttackPress { get; set; }

    public Dictionary<Type, IComponent> EnemyComponents => _enemyComponents;
    private void Awake()
    {
        _target ??= FindObjectOfType<PlayerController>().transform; 

        _moveModule = GetComponent<AgentMoveModule<Enemy>>();
        _attackModule = GetComponent<AttackModule>();
        _fov = GetComponent<FieldOfView>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _hpModule = GetComponent<HPModule>();

        SetComponents();
    }

    private void Start()
    {
        _enemyTree = new EnemyTree<Enemy>(this);
        _hpModule.Init(_enemySO.hp, _enemySO.hp);
    }

    private void Update()
    {
        // 디버그용 (임시)
        IsFind = CheckChase();
        Debug.DrawLine(transform.position, transform.position + _fov.GetVecByAngle(_enemySO.eyeAngle / 2, false) * _enemySO.chaseDistance,Color.red);
        Debug.DrawLine(transform.position, transform.position + _fov.GetVecByAngle(-_enemySO.eyeAngle / 2, false) * _enemySO.chaseDistance, Color.red);
        
        _enemyTree.UpdateRun();
    }

    private void SetComponents()
    {
        _enemyComponents.Add(typeof(AgentMoveModule<Enemy>), _moveModule);
        _enemyComponents.Add(typeof(AttackModule), _attackModule);
        _enemyComponents.Add(typeof(FieldOfView), _fov);
        _enemyComponents.Add(typeof(EnemyAnimation), _enemyAnimation);
    }

    public bool IsFind = false;



    // 피격 관련 
    public void GetDamaged(int damage, GameObject damageDealer)
    {
        Debug.Log($"{transform.name} 피격");
        _hpModule.ChangeHP(-damage); 
    }

    #region Condition

    // 죽었는가
    public bool IsDie()
    {
        Debug.Log("죽음체크");
        return _hpModule.HP <= 0; 
    }
    // 피격 받은 상태인가 
    public bool IsHit()
    {
        Debug.Log("피격체크");
        return _isHit;
    }
    // 전투 중인가 
    public bool IsbattleMode()
    {
        Debug.Log("전투중 체크");
        return _isBattleMode; 
    }
    // 공격 범위 안에 들어왔는가
    public bool CheckAttack()
    {
        Debug.Log("공격 범위 체크");
        return CheckDistance(_enemySO.eyeAngle, _enemySO.attackDistance);
    }

    /// <summary>
    ///  추격 거리 체크 
    /// </summary>
    /// <returns></returns>
    public bool CheckChase()
    {
        return CheckDistance(_enemySO.eyeAngle, _enemySO.chaseDistance);
    }

    public void OnDie()
    {
    }

    /// <summary>
    /// 거리 체크 
    /// </summary>
    /// <param name="eyeAngle"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private bool CheckDistance(float eyeAngle, float distance)
    {
        if (_target == null)
        {
            Debug.LogError("타겟 없음");
            return false;
        }
        Vector3 targetDir = (_target.transform.position - transform.position).normalized; // 타겟 방향  

        if (Vector3.Angle(transform.forward, targetDir) > eyeAngle * 0.5f // 시야 범위 안에 있고 
            && Vector3.Distance(_target.position, transform.position) < distance) // 거리 안에 있으면 
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Action

    /// <summary>
    /// 기본 공격 
    /// </summary>
    public void DefaultAttack()
    {
        Debug.Log("공격"); 
    }

    /// <summary>
    /// 추적 
    /// </summary>
    public void Chase()
    {
        Debug.Log("추적..");
    }
    #endregion

}
