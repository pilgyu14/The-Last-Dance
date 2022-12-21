using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rito.BehaviorTree;

using static Rito.BehaviorTree.NodeHelper;
using System;

public class Enemy : PoolableMono, IDamagable, IAgent, IAgentInput, IKnockback
{
    protected EnemyTree<Enemy> _enemyTree;

    [SerializeField]
    protected EnemySO _enemySO;
    [SerializeField]
    protected LayerMask _groundLayerMask;
    protected Transform _target; // �⺻������ �Ͱ� ���� ��� ó���� ���� ���� 
    protected Transform _battleTarget; // ���� Ÿ���� ã�Ҵ°��� ���� ���� 

    protected Dictionary<Type, IComponent> _enemyComponents = new Dictionary<Type, IComponent>();  // ��� ������Ʈ ���� ��ųʸ� 

    protected HPModule _hpModule;
    // ĳ�� ���� 
    protected EnemyMoveModule _moveModule;
    protected AttackModule _attackModule;
    protected FieldOfView _fov;
    protected NavMeshAgent _agent;
    protected EnemyAnimation _enemyAnimation;
    protected AgentAudioPlayer _audioPlayer;
    protected CapsuleCollider _collider;
    protected Rigidbody _rigid;
    private ItemDropper _itemDroper; 

    // ���� ���� 
    protected bool _isHit = false; // �ǰ����ΰ�
    protected bool _isAttacking = false; // �������ΰ� 
    protected bool _isStunned = false; // �����߳� 


    // Collider ����
    protected Vector3 _originColCenter;
    protected float _originColHeight;

    protected Vector3 _hitPoint;
    // ������Ƽ 
    public bool IsBattleMode
    {
        get => _isAttacking;
        set
        {
            _isAttacking = value;
        }
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

    protected virtual void Awake()
    {
        _target ??= FindObjectOfType<PlayerController>().transform; 

        _moveModule = GetComponent<EnemyMoveModule>();
        _attackModule = GetComponentInChildren<AttackModule>();
        _fov = GetComponent<FieldOfView>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        _hpModule = GetComponent<HPModule>();
        _audioPlayer = GetComponentInChildren<AgentAudioPlayer>();
        _collider = GetComponent<CapsuleCollider>();
        _rigid = GetComponent<Rigidbody>();
        _itemDroper = GetComponentInChildren<ItemDropper>(); 

        SetComponents();

        // ��� �ʱ�ȭ
        _hpModule.Init(_enemySO.maxHp, _enemySO.maxHp);
        _moveModule.Init(this, _agent, _enemySO.moveInfo);
        _attackModule.Init(this, _fov, _moveModule, _enemyAnimation);
    }

    protected virtual void Start()
    {
         CreateTree(); 



        _agent.speed = _enemySO.moveInfo.maxSpeed;

        _originColCenter = _collider.center;
        _originColHeight = _collider.height;
    }

    protected virtual void Update()
    {
        // ����׿� (�ӽ�)
        Debug.DrawLine(transform.position, transform.position + _fov.GetVecByAngle(_enemySO.eyeAngle / 2, false) * _enemySO.chaseDistance,Color.cyan);
        Debug.DrawLine(transform.position, transform.position + _fov.GetVecByAngle(-_enemySO.eyeAngle / 2, false) * _enemySO.chaseDistance, Color.cyan);

        Debug.DrawLine(transform.position, transform.position + _fov.GetVecByAngle(_enemySO.eyeAngle / 2, false) * _enemySO.attackDistance, Color.red);
        Debug.DrawLine(transform.position, transform.position + _fov.GetVecByAngle(-_enemySO.eyeAngle / 2, false) * _enemySO.attackDistance, Color.red);

        // �ִϸ��̼� ���� 
        _enemyAnimation.AnimatePlayer(_agent.velocity.sqrMagnitude);

        UpdateTree(); 
    }


    /// <summary>
    /// �����̺�� Ʈ�� ����
    /// </summary>
    protected virtual void CreateTree()
    {
        _enemyTree = new EnemyTree<Enemy>(this);
    }

    /// <summary>
    /// �����̺�� Ʈ�� ������Ʈ 
    /// </summary>
    protected virtual void UpdateTree()
    {
        _enemyTree.UpdateRun();
    }

    protected void SetComponents()
    {
        _enemyComponents.Add(typeof(EnemyMoveModule), _moveModule);
        _enemyComponents.Add(typeof(AttackModule), _attackModule);
        _enemyComponents.Add(typeof(FieldOfView), _fov);
        _enemyComponents.Add(typeof(EnemyAnimation), _enemyAnimation);
    }

    public bool IsFind = false;


    // �ǰ� ���� 
    public virtual void GetDamaged(int damage, GameObject damageDealer)
    {
        Debug.LogError($"{transform.name} �ǰ�, ������ {damage}");
        if (_hpModule.ChangeHP(-damage) == false)
        { 
            OnDie();
            damageDealer.GetComponent<PlayerController>().PlayerSO.CalculateExp(_enemySO.level); 
        }
        _enemyAnimation.PlayHitAnimation(); 
        _isHit = true; // �¾Ҵ�
        _audioPlayer.PlayClip(_enemySO.hitClip);
        // ���� 

        // Ÿ�� �ٶ󺸱� 
    }

    public void Knockback(Vector3 direction, float power, float duration)
    {
        StartCoroutine(_moveModule.DashCorutine(direction, power, duration)); 
     }

    public virtual void Die()
    {

        _agent.enabled = false; 
        _collider.center = new Vector3(0, 1.78f, 0);
        _collider.height = 1.1f; 
        _rigid.isKinematic = false;

        StartCoroutine(CheckGravity()); 
    }

    /// <summary>
    /// �ٴ����� ���������� �ٽ� �������� �ȵǵ��� ( �÷��̾�� �浹�ż� �� ������ ��������. ) 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator CheckGravity()
    {
        Vector3 origin = transform.position + _collider.center; 
        while(true)
        {
            origin = transform.position + _collider.center; 
            Debug.DrawRay(origin, Vector3.down, Color.red,0.5f); 
            if(Physics.Raycast(origin, Vector3.down, 0.5f, _groundLayerMask) == true)
            {
                _rigid.isKinematic = true;
                break;
            }
            yield return null; 
        }
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
    public bool CheckMeleeAttack()
    {
        float atkDistance = _attackModule.CurAttackBase.attackCollider.GetAtkRange();
        Debug.Log("���� ���� üũ");
        return CheckDistance(_enemySO.eyeAngle, atkDistance);
    }
    // �⺻ ��ġ�� �ִ°� 
    public bool IsOriginPos()
    {
        Debug.Log("�⺻ ��ġ�� �ִ°�");
        return _moveModule.IsOriginPos(); 
    }

    // �⺻ ���� ��Ÿ�� 
    public bool CheckDefaultAttackCoolTime()
    {
       
        Debug.Log("�⺻ ���� ��Ÿ�� üũ");
        return _attackModule.GetAttackInfo(AttackType.Default_1).IsCoolTime;
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

    public virtual void OnDie()
    {
        _enemyAnimation.PlayDeathAnimation();
        StartCoroutine(Destroy()); //  ���� �ð� �� ����  
        _itemDroper.DropItemAndSkill(); 
        _audioPlayer.PlayClip(_enemySO.deathClip);
        GameManager.Instance.MonsterDie(); 
        // ������ ������
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5f);
        PoolManager.Instance.Push(this); 
    }

    /// <summary>
    /// �Ÿ� üũ 
    /// </summary>
    /// <param name="eyeAngle"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    protected bool CheckDistance(float eyeAngle, float distance)
    {
        if (Target == null)
        {
            Debug.LogError("Ÿ�� ����");
            return false;
        }
        Vector3 targetDir = (Target.transform.position - transform.position).normalized; // Ÿ�� ����  

        //Debug.DrawLine(transform.position, targetDir )
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

    protected void DefaultAttack(AttackType attackType)
    {
        _isAttacking = true; 
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


    // ���� �ִϸ��̼� ������ �� ���� 
    public void EndAttack()
    {
        _isAttacking = false; 
    }

    // �ǰ� �ִϸ��̼� ������ �� ���� 
    public void EndHit()
    {
        _isHit = false; 
    }

    public override void Reset()
    {
        _collider.center = _originColCenter;
        _collider.height = _originColHeight;
        _agent.enabled = true;
        _rigid.isKinematic = true;

        _hpModule.Init(_enemySO.maxHp, _enemySO.maxHp);

        _enemyAnimation.AgentAnimator.Rebind();
        _moveModule.SetOriginPos(); 
    }
}
