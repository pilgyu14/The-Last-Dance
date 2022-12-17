using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ���� �¿� �̵� 
// ȸ�� 
// �׼� 
// �ִϸ��̼� 

#region FSM
public enum PlayerStateType
{
    DefaultType,
    InBattleType,
}

public abstract class State<T>
{
    protected T owner;


    public virtual void Init(T owner)
    {
        this.owner = owner;
    }
    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();

}

public class DefaultState : State<PlayerController>
{

    public override void Enter()
    {
        owner.InputModule.OnMovementKeyPress += owner.MoveModule.Move;
        owner.InputModule.OnMoveAnimation += owner.MoveDefaultAnimation;
        // move �ֱ� 
    }
    public override void Stay()
    {

    }

    public override void Exit()
    {
        owner.InputModule.OnMovementKeyPress -= owner.MoveModule.Move;
        owner.InputModule.OnMoveAnimation -= owner.MoveDefaultAnimation;
    }

}

public class InBattleState : State<PlayerController>
{
    private TimerModule timer;
    public override void Enter()
    {
        owner.StartBattle();
        owner.InputModule.OnMovementKeyPress += owner.MoveModule.InBattleMove;
        timer = new TimerModule(1f, () => owner.SetBattle());
        // InBattlemove �ֱ� 
        // StartBattle
    }
    public override void Stay()
    {
        owner.CheckBattle();
        //CheckBattle
    }

    public override void Exit()
    {
        owner.InputModule.OnMovementKeyPress -= owner.MoveModule.InBattleMove;
        timer = null;
    }
}

public class AttackState : State<PlayerController>
{
    TimerModule timer;
    private AttackType _curAttackType;
    private AttackType _nextAttackType;

    public AttackType NextAttackType => _nextAttackType;
    public AttackType CurAttackType => _curAttackType;
    public override void Enter()
    {
        owner.StartAttack();
        _nextAttackType = owner.AttackModule.NextAttackType;
        _curAttackType = owner.AttackModule.CurAttackType;
        timer = new TimerModule(3.5f, () => owner.SetAttack());
        // �ð� ī��Ʈ 
        // ���� 
    }

    public override void Stay()
    {
        timer.UpdateSomething();
    }

    public override void Exit()
    {
        timer = null;
    }
}

#endregion
public class PlayerController : MonoBehaviour, IAgent, IDamagable
{
    #region ���� 
    // �ν����� 
    [SerializeField]
    private PlayerSO _playerSO;

    // ĳ�� ���� 
    private InputModule _inputModule;
    private PlayerMoveModule _moveModule;
    private AttackModule<PlayerController> _attackModule;
    private FieldOfView _fov;
    private CharacterController _chController;
    private NavMeshAgent _agent;
    private PlayerAnimation _playerAnimation;
    private HPModule _hpModule;
    private AgentAudioPlayer _autioPlayer;

    // ���� ���� 
    #region State
    private Dictionary<Type, State<PlayerController>> _stateDic = new Dictionary<Type, State<PlayerController>>();

    private DefaultState _defaultState;
    private InBattleState _inBattleState;
    private AttackState _attackState;

    private State<PlayerController> _curState;
    private State<PlayerController> _prevState;

    public State<PlayerController> CurState => _curState;
    #endregion

    private Vector3 _targetPos;
    private Quaternion _targetRot;

    private bool _isDie = false;
    [SerializeField]
    private bool _isBattle = false; // ���������ΰ� 
    private bool _isAttack = false; // ���� ���ΰ� ( 1Ÿ 2Ÿ 3Ÿ )  ���� ���� ���� 

    // ������Ƽ 
    public InputModule InputModule => _inputModule;
    public PlayerMoveModule MoveModule => _moveModule;
    public AttackModule<PlayerController> AttackModule => _attackModule;
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    public bool IsEnemy => false;
    public Vector3 HitPoint => throw new NotImplementedException();

    public bool IsAttack
    {
        get => _isAttack;
        set => _isAttack = value;
    }

    public AgentAudioPlayer AudioPlayer => _autioPlayer;

    public NavMeshAgent NavMeshAgent => _agent;

    public GameObject obj => gameObject;
    #endregion

    #region �ʱ�ȭ 
    private void Awake()
    {
        _inputModule = GetComponentInChildren<InputModule>();
        _moveModule = GetComponent<PlayerMoveModule>();
        _attackModule = GetComponentInChildren<AttackModule<PlayerController>>();
        _fov = GetComponent<FieldOfView>();
        //_chController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _hpModule = GetComponent<HPModule>();
        _autioPlayer = GetComponentInChildren<AgentAudioPlayer>();
    }

    private void Start()
    {
        // ���� ��� 
        _defaultState = new DefaultState();
        _inBattleState = new InBattleState();
        _attackState = new AttackState();

        _defaultState.Init(this);
        _inBattleState.Init(this);
        _attackState.Init(this);

        _stateDic.Add(_defaultState.GetType(), _defaultState);
        _stateDic.Add(_inBattleState.GetType(), _inBattleState);
        _stateDic.Add(_attackState.GetType(), _attackState);

        ChangeState(typeof(DefaultState));

        // �Է� ��� 
        _inputModule.Init(this);
        _inputModule.OnDefaultAttackPress = DefaultKickAttack;
        _inputModule.OnShift = TackeAttack;


        //_inputModule.OnPointerRotate = RotateByMouse;
        //_inputModule.OnMovementKeyPress = Move;
        // _inputModule.OnMovementKeyPress = InBattleMove; 

        _playerSO.UpdateStat();
        // ��� �ʱ�ȭ
        _moveModule.Init(this, _agent, _playerSO.moveInfo, _playerAnimation, _inputModule);
        _attackModule.Init(this, _fov, _moveModule, _playerAnimation);
        _hpModule.Init(_playerSO.hp, _playerSO.hp);
    }
    #endregion
    private void Update()
    {
        //Debug.Log(_curState.GetType().Name); 
        // if (_isAttack == true) return;

        _curState.Stay();

        //if (_isBattle == false)
        //{
        //    Move();
        //    return; 
        //}
        //InBattleMove(); 
    }

    public void ChangeState(Type type)
    {
        if (_stateDic.ContainsKey(type) == false)
        {
            Debug.LogError("�������� �ʴ� ����");
            return;
        }

        if (_curState != null && _curState != _stateDic[_curState.GetType()] && _curState.GetType() != typeof(AttackState))
        {
            return;
        }

        Debug.Log("OK1");
        if (_curState != null)
        {
            _prevState = _stateDic[_curState.GetType()];
            _prevState.Exit();
        }

        _curState = _stateDic[type];
        _curState.Enter();
    }

    public void MoveDefaultAnimation(Vector3 v)
    {
        _playerAnimation.AnimatePlayer(v.sqrMagnitude);
        //_playerAnimation.AnimatePlayer(_agent.velocity.magnitude);

    }

    [SerializeField]
    private float _time = 0f;  // ���� ����X �ð� 
    public void CheckBattle()
    {
        //Debug.Log("d"); 
        if (_isBattle == false) return;

        _time += Time.deltaTime;
        if (_isBattle == true && _time >= 5f) // �������°� �����ð� ���ӵ��� �ʾ����� 
        {
            _isBattle = false;
            _time = 0;
            _playerAnimation.SetBattle(_isBattle);

            SetState(typeof(DefaultState), ref _isBattle);

        }
    }

    public void CheckAttack()
    {
        if (_isBattle == false) return;

        _time += Time.deltaTime;
        if (_isBattle == true && _time >= 1f) // �������°� �����ð� ���ӵ��� �ʾ����� 
        {
            _time = 0;
            SetState(typeof(InBattleState), ref _isBattle);

        }
    }

    public void SetBattle()
    {
        SetState(typeof(DefaultState), ref _isBattle);
    }

    public void SetAttack()
    {
        SetState(typeof(InBattleState), ref _isAttack);
    }

    private void SetState(Type state, ref bool isActive)
    {
        isActive = false;
        ChangeState(state);
    }

    public void StartBattle()
    {
        _time = 0;
        _isBattle = true;
        _playerAnimation.SetBattle(_isBattle);
    }

    public void StartAttack()
    {
        _time = 0;
        _isBattle = true;
        _playerAnimation.SetBattle(_isBattle);
        _isAttack = true;
    }

    private bool _isDelay = false; // �⺻ ������ �� �ִ°� 
    [SerializeField]
    private float _curTime = 0f;
    private float _maxCoolTime = 0.7f;  // �⺻ ���� 3Ÿ �� ������ 

    IEnumerator Delay(float delayTime)
    {
        _isDelay = true;
        while (_curTime <= delayTime)
        {
            _curTime += Time.deltaTime;
            yield return null;
        }
        Debug.LogError("������ ��");
        _isDelay = false;
    }

    private void DefaultKickAttack()
    {
        if (_isDelay == true) return;  // ������ üũ 
        // �⺻ �������� üũ�� 
        // ���� �ִϸ��̼��� �󸶳� ���������� Ȯ�� 

        // ���ݾִϸ��̼� �������̸鼭 ���� �ð��̻� ����� ���°� �ƴ϶�� 
        if (_playerAnimation.CheckDefaultAnim() == true &&
                        _playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            Debug.Log("  ���� �ȵſ� ");
            return;
        }
        ChangeAttackStateType(); 
        if (_attackModule.NextAttackType == AttackType.Null) // ��Ŭ���� ��ų�� ��� 
        {
            _attackModule.SetCurAttackType(AttackType.Default_1);
        }
        _attackModule.DefaultAttack(); // �������� ���� ����( ���� üũ �� Ÿ��, �ִϸ��̼� ���� ) 

        // ���� ���� �� ���ݻ��·� ���� 
        ChangeState(typeof(AttackState));
        AttackState attackState = _curState as AttackState;
    }

    /// <summary>
    /// ���� ���¿� ���� ���� ���� ( �ִϸ��̼� ������ ������ ���� ) 
    /// </summary>
    public void ChangeAttackStateType()
    {
        if (_isAttack == true) // 1�ʰ� �⺻���� �� ���¸� ���� �������� �̾ ���� 
        {
            if (_attackModule.CurAttackType == AttackType.Default_3) // 3��° �����̸� �������ֱ�
            {
                _curTime = 0;
                StartCoroutine(Delay(_maxCoolTime));
            }
                //���� ���� ����
                _attackModule.SetCurAttackType(_attackModule.NextAttackType);
        }
        else 
        {
            // default_1 ���� 
            _attackModule.SetCurAttackType(AttackType.Default_1);
        }
    }

    private void TackeAttack()
    {
        _attackModule.SetCurAttackType(AttackType.Tackle);
        _attackModule.DefaultAttack();
    }

    // �ǰ� ���� 
    public void Damaged()
    {
    }

    public void OnDie()
    {

    }

    public bool IsDie()
    {
        return _isDie;
    }

    public void GetDamaged(int damage, GameObject damageDealer)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// ������ ( �÷��̾� �Է� ���� )
    /// </summary>
    public void Attacking()
    {
        _inputModule.Attacking(true);
    }

    /// <summary>
    /// �⺻ �����̸� �Է� ���� + �뽬 üũ
    /// </summary>
    public void DefaultAttacking()
    {
        Attacking();
        _moveModule.StopMove();
        _moveModule.Dash();
    }

    /// <summary>
    /// ���� ��
    /// </summary>
    public void EndAttacking()
    {
        _inputModule.Attacking(false);
        //ChangeAttackStateType(); 
    }

}
