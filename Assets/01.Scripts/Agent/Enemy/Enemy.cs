using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rito.BehaviorTree;

using static Rito.BehaviorTree.NodeHelper;
using System;

public class Enemy : MonoBehaviour, IDamagable, IAgent, IAgentInput, IKnockback
{
    [SerializeField]
    private EnemySO _enemySO;
    private Transform _target; // 기본적으로 터겟 관련 모든 처리를 위한 변수 
    private Transform _battleTarget; // 현재 타겟을 찾았는가에 관한 변수 
    private EnemyTree<Enemy> _enemyTree;

    private Dictionary<Type, IComponent> _enemyComponents = new Dictionary<Type, IComponent>();  // 모든 컴포넌트 저장 딕셔너리 

    private HPModule _hpModule;
    // 캐싱 변수 
    private EnemyMoveModule _moveModule;
    private AttackModule _attackModule;
    private FieldOfView _fov;
    private NavMeshAgent _agent;
    private EnemyAnimation _enemyAnimation;
    private AgentAudioPlayer _audioPlayer; 

    // 상태 변수 
    private bool _isHit = false; // 피격중인가
    private bool _isAttacking = false; // 전투중인가 
    private bool _isStunned = false; // 기절했냐 

    private Vector3 _hitPoint;
    // 프로퍼티 
    public bool IsBattleMode
    {
        get => _isAttacking;
        set => _isAttacking = value; 
    }

    public Transform Target
    {
        get
        {
            if (_target == null) _target = FindObjectOfType<PlayerController>().transform;
            return _target; 
        }
        }

    public bool IsEnemy => true;
    public Vector3 HitPoint => _hitPoint;

    public Action<Vector3> OnMovementKeyPress { get; set; }
    public Action<Vector3> OnPointerRotate { get; set; }
    public Action OnDefaultAttackPress { get; set; }

    public Dictionary<Type, IComponent> EnemyComponents => _enemyComponents;

    public AgentAudioPlayer AudioPlayer => _audioPlayer;

    public NavMeshAgent NavMeshAgent => _agent;

    public GameObject obj => gameObject;

    private void Awake()
    {
        _target ??= FindObjectOfType<PlayerController>().transform; 

        _moveModule = GetComponent<EnemyMoveModule>();
        _attackModule = GetComponentInChildren<AttackModule>();
        _fov = GetComponent<FieldOfView>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        _hpModule = GetComponent<HPModule>();
        _audioPlayer = GetComponentInChildren<AgentAudioPlayer>();

        SetComponents();
    }

    private void Start()
    {
        _enemyTree = new EnemyTree<Enemy>(this);
        _hpModule.Init(_enemySO.hp, _enemySO.hp);

        _moveModule.Init(this, _agent, _enemySO.moveInfo);
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
        Debug.LogError($"{transform.name} 피격, 데미지 {damage}");
        _enemyAnimation.PlayHitAnimation(); 
        _hpModule.ChangeHP(-damage);
        _isHit = true; // 맞았다

        // 타겟 바라보기 
    }

    public void Knockback(Vector3 direction, float power, float duration)
    {
        StartCoroutine(_moveModule.DashCorutine(direction, power, duration)); 
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
    public bool IsAttacking()
    {
        Debug.Log("전투중 체크" + _isAttacking);
        return _isAttacking; 
    }
    // 타겟을 발견하지 않았고 피격 당했으면 
    public bool IsFirstHit()
    {
        bool isFirstHit = _isHit && _battleTarget == null;
        if (isFirstHit == false) _isHit = false; // 피격 체크 후 바로 false 
        return isFirstHit ; 
    }

    // 기절 상태냐 
    public bool IsStunned()
    {
        Debug.Log("기절상태 체크");
        return _isStunned;
    }
    // 공격 범위 안에 들어왔는가
    public bool CheckAttack()
    {
        Debug.Log("공격 범위 체크");
        return CheckDistance(_enemySO.eyeAngle, _enemySO.attackDistance);
    }
    // 기본 위치에 있는가 
    public bool IsOriginPos()
    {
        Debug.Log("기본 위치에 있는가");
        return _moveModule.IsOriginPos(); 
    }

    // 기본 공격 쿨타임 
    public bool CheckCoolTime()
    {
        Debug.Log("기본 공격 쿨타임 체크");
        return false;
    }
    /// <summary>
    ///  추격 거리 체크 
    /// </summary>
    /// <returns></returns>
    public bool CheckChase()
    {
        Debug.Log("추적 거리 체크");
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
        if (Target == null)
        {
            Debug.LogError("타겟 없음");
            return false;
        }
        Vector3 targetDir = (Target.transform.position - transform.position).normalized; // 타겟 방향  

        if (Vector3.Angle(transform.forward, targetDir) < eyeAngle * 0.5f // 시야 범위 안에 있고 
            && Vector3.Distance(Target.position, transform.position) < distance) // 거리 안에 있으면 
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Action


    // @@@ 공격 @@@ //
    /// <summary>
    /// 기본 공격 
    /// </summary>
    public void DefaultAttack_1()
    {
        // _enemyAnimation.
        DefaultAttack(AttackType.Default_1);

    }

    public void DefaultAttack_2()
    {
        DefaultAttack(AttackType.Default_2);

    }
    public void DefaultAttack_3()
    {
        DefaultAttack(AttackType.Default_3);
    }

    private void DefaultAttack(AttackType attackType)
    {
        _attackModule.SetCurAttackType(attackType);
        _attackModule.DefaultAttack();
        Debug.Log("공격" + attackType.GetType().Name);
    }
    // @@@ 이동 @@@ //
    /// <summary>
    /// 추적 
    /// </summary>
    public void Chase()
    {
        // 타겟 찾았다 
        _battleTarget = _target; 
        // 애니메이션 실행 
        _enemyAnimation.AnimatePlayer(_agent.velocity.sqrMagnitude);
        // 추적 시작 
        _moveModule.Chase(); 
        Debug.Log("추적..");
    }

    public void MoveOrigin()
    {
        if(_battleTarget != null)
            _battleTarget = null; 
        
        _moveModule.MoveOriginPos();
        Debug.Log("기본 위치로 이동");
    }

    // @@@ 회전 @@@ //
    public void Idle()
    {
        // 180도 기준으로 돈다 
        _moveModule.RotateIdle(); 
        Debug.Log("기본 상태..");
    }

    // 타겟 바라보기 
    public void LookTarget()
    {
        _moveModule.RotateByPos(_target.position); 
    }



    #endregion

}
