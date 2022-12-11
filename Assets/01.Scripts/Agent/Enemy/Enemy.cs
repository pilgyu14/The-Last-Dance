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

    private Dictionary<Type, IComponent> _enemyComponents = new Dictionary<Type, IComponent>();  // ��� ������Ʈ ���� ��ųʸ� 

    private HPModule _hpModule; 
    // ĳ�� ���� 
    private AgentMoveModule<Enemy> _moveModule;
    private AttackModule _attackModule;
    private FieldOfView _fov;
    private NavMeshAgent _agent;
    private EnemyAnimation _enemyAnimation;

    // ���� ���� 
    private bool _isHit = false; // �ǰ����ΰ�
    private bool _isBattleMode = false; // �������ΰ� 

    private Vector3 _hitPoint;
    // ������Ƽ 
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
        Debug.Log($"{transform.name} �ǰ�");
        _hpModule.ChangeHP(-damage); 
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
    public bool IsbattleMode()
    {
        Debug.Log("������ üũ");
        return _isBattleMode; 
    }
    // ���� ���� �ȿ� ���Դ°�
    public bool CheckAttack()
    {
        Debug.Log("���� ���� üũ");
        return CheckDistance(_enemySO.eyeAngle, _enemySO.attackDistance);
    }

    /// <summary>
    ///  �߰� �Ÿ� üũ 
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
    /// �Ÿ� üũ 
    /// </summary>
    /// <param name="eyeAngle"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private bool CheckDistance(float eyeAngle, float distance)
    {
        if (_target == null)
        {
            Debug.LogError("Ÿ�� ����");
            return false;
        }
        Vector3 targetDir = (_target.transform.position - transform.position).normalized; // Ÿ�� ����  

        if (Vector3.Angle(transform.forward, targetDir) > eyeAngle * 0.5f // �þ� ���� �ȿ� �ְ� 
            && Vector3.Distance(_target.position, transform.position) < distance) // �Ÿ� �ȿ� ������ 
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Action

    /// <summary>
    /// �⺻ ���� 
    /// </summary>
    public void DefaultAttack()
    {
        Debug.Log("����"); 
    }

    /// <summary>
    /// ���� 
    /// </summary>
    public void Chase()
    {
        Debug.Log("����..");
    }
    #endregion

}
