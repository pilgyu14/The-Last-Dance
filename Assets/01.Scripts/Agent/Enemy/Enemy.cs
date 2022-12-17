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
    private Transform _target; // �⺻������ �Ͱ� ���� ��� ó���� ���� ���� 
    private Transform _battleTarget; // ���� Ÿ���� ã�Ҵ°��� ���� ���� 
    private EnemyTree<Enemy> _enemyTree;

    private Dictionary<Type, IComponent> _enemyComponents = new Dictionary<Type, IComponent>();  // ��� ������Ʈ ���� ��ųʸ� 

    private HPModule _hpModule;
    // ĳ�� ���� 
    private EnemyMoveModule _moveModule;
    private AttackModule _attackModule;
    private FieldOfView _fov;
    private NavMeshAgent _agent;
    private EnemyAnimation _enemyAnimation;
    private AgentAudioPlayer _audioPlayer; 

    // ���� ���� 
    private bool _isHit = false; // �ǰ����ΰ�
    private bool _isAttacking = false; // �������ΰ� 
    private bool _isStunned = false; // �����߳� 

    private Vector3 _hitPoint;
    // ������Ƽ 
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
        // ����׿� (�ӽ�)
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



    // �ǰ� ���� 
    public void GetDamaged(int damage, GameObject damageDealer)
    {
        Debug.LogError($"{transform.name} �ǰ�, ������ {damage}");
        _enemyAnimation.PlayHitAnimation(); 
        _hpModule.ChangeHP(-damage);
        _isHit = true; // �¾Ҵ�

        // Ÿ�� �ٶ󺸱� 
    }

    public void Knockback(Vector3 direction, float power, float duration)
    {
        StartCoroutine(_moveModule.DashCorutine(direction, power, duration)); 
     }

  
    #region Condition

    // �׾��°�
    public bool IsDie()
    {
        Debug.Log("����üũ");
        return _hpModule.HP <= 0; 
    }
    // �ǰ� ���� �����ΰ� 
    public bool IsHit()
    {
        Debug.Log("�ǰ�üũ");
        return _isHit;
    }
    // ���� ���ΰ� 
    public bool IsAttacking()
    {
        Debug.Log("������ üũ" + _isAttacking);
        return _isAttacking; 
    }
    // Ÿ���� �߰����� �ʾҰ� �ǰ� �������� 
    public bool IsFirstHit()
    {
        bool isFirstHit = _isHit && _battleTarget == null;
        if (isFirstHit == false) _isHit = false; // �ǰ� üũ �� �ٷ� false 
        return isFirstHit ; 
    }

    // ���� ���³� 
    public bool IsStunned()
    {
        Debug.Log("�������� üũ");
        return _isStunned;
    }
    // ���� ���� �ȿ� ���Դ°�
    public bool CheckAttack()
    {
        Debug.Log("���� ���� üũ");
        return CheckDistance(_enemySO.eyeAngle, _enemySO.attackDistance);
    }
    // �⺻ ��ġ�� �ִ°� 
    public bool IsOriginPos()
    {
        Debug.Log("�⺻ ��ġ�� �ִ°�");
        return _moveModule.IsOriginPos(); 
    }

    // �⺻ ���� ��Ÿ�� 
    public bool CheckCoolTime()
    {
        Debug.Log("�⺻ ���� ��Ÿ�� üũ");
        return false;
    }
    /// <summary>
    ///  �߰� �Ÿ� üũ 
    /// </summary>
    /// <returns></returns>
    public bool CheckChase()
    {
        Debug.Log("���� �Ÿ� üũ");
        return CheckDistance(_enemySO.eyeAngle, _enemySO.chaseDistance);
    }

    public void OnDie()
    {
    }

    /// <summary>
    /// �Ÿ� üũ 
    /// </summary>
    /// <param name="eyeAngle"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private bool CheckDistance(float eyeAngle, float distance)
    {
        if (Target == null)
        {
            Debug.LogError("Ÿ�� ����");
            return false;
        }
        Vector3 targetDir = (Target.transform.position - transform.position).normalized; // Ÿ�� ����  

        if (Vector3.Angle(transform.forward, targetDir) < eyeAngle * 0.5f // �þ� ���� �ȿ� �ְ� 
            && Vector3.Distance(Target.position, transform.position) < distance) // �Ÿ� �ȿ� ������ 
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Action


    // @@@ ���� @@@ //
    /// <summary>
    /// �⺻ ���� 
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
        Debug.Log("����" + attackType.GetType().Name);
    }
    // @@@ �̵� @@@ //
    /// <summary>
    /// ���� 
    /// </summary>
    public void Chase()
    {
        // Ÿ�� ã�Ҵ� 
        _battleTarget = _target; 
        // �ִϸ��̼� ���� 
        _enemyAnimation.AnimatePlayer(_agent.velocity.sqrMagnitude);
        // ���� ���� 
        _moveModule.Chase(); 
        Debug.Log("����..");
    }

    public void MoveOrigin()
    {
        if(_battleTarget != null)
            _battleTarget = null; 
        
        _moveModule.MoveOriginPos();
        Debug.Log("�⺻ ��ġ�� �̵�");
    }

    // @@@ ȸ�� @@@ //
    public void Idle()
    {
        // 180�� �������� ���� 
        _moveModule.RotateIdle(); 
        Debug.Log("�⺻ ����..");
    }

    // Ÿ�� �ٶ󺸱� 
    public void LookTarget()
    {
        _moveModule.RotateByPos(_target.position); 
    }



    #endregion

}
