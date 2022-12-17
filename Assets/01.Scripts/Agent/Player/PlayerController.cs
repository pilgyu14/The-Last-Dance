using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 상하 좌우 이동 
// 회전 
// 액션 
// 애니메이션 

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
        // move 넣기 
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
        // InBattlemove 넣기 
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
        // 시간 카운트 
        // 공격 
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
    #region 변수 
    // 인스펙터 
    [SerializeField]
    private PlayerSO _playerSO;

    // 캐싱 변수 
    private InputModule _inputModule;
    private PlayerMoveModule _moveModule;
    private AttackModule<PlayerController> _attackModule;
    private FieldOfView _fov;
    private CharacterController _chController;
    private NavMeshAgent _agent;
    private PlayerAnimation _playerAnimation;
    private HPModule _hpModule;
    private AgentAudioPlayer _autioPlayer;

    // 내부 변수 
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
    private bool _isBattle = false; // 전투상태인가 
    private bool _isAttack = false; // 공격 중인가 ( 1타 2타 3타 )  연속 공격 여부 

    // 프로퍼티 
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

    #region 초기화 
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
        // 상태 등록 
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

        // 입력 등록 
        _inputModule.Init(this);
        _inputModule.OnDefaultAttackPress = DefaultKickAttack;
        _inputModule.OnShift = TackeAttack;


        //_inputModule.OnPointerRotate = RotateByMouse;
        //_inputModule.OnMovementKeyPress = Move;
        // _inputModule.OnMovementKeyPress = InBattleMove; 

        _playerSO.UpdateStat();
        // 모듈 초기화
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
            Debug.LogError("존재하지 않는 상태");
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
    private float _time = 0f;  // 전투 지속X 시간 
    public void CheckBattle()
    {
        //Debug.Log("d"); 
        if (_isBattle == false) return;

        _time += Time.deltaTime;
        if (_isBattle == true && _time >= 5f) // 전투상태가 일정시간 지속되지 않았을때 
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
        if (_isBattle == true && _time >= 1f) // 전투상태가 일정시간 지속되지 않았을때 
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

    private bool _isDelay = false; // 기본 공격할 수 있는가 
    [SerializeField]
    private float _curTime = 0f;
    private float _maxCoolTime = 0.7f;  // 기본 공격 3타 후 딜레이 

    IEnumerator Delay(float delayTime)
    {
        _isDelay = true;
        while (_curTime <= delayTime)
        {
            _curTime += Time.deltaTime;
            yield return null;
        }
        Debug.LogError("딜레이 끝");
        _isDelay = false;
    }

    private void DefaultKickAttack()
    {
        if (_isDelay == true) return;  // 딜레이 체크 
        // 기본 공격인지 체크후 
        // 현재 애니메이션이 얼마나 실행중인지 확인 

        // 공격애니메이션 실행중이면서 일정 시간이상 실행된 상태가 아니라면 
        if (_playerAnimation.CheckDefaultAnim() == true &&
                        _playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            Debug.Log("  공격 안돼요 ");
            return;
        }
        ChangeAttackStateType(); 
        if (_attackModule.NextAttackType == AttackType.Null) // 태클같은 스킬인 경우 
        {
            _attackModule.SetCurAttackType(AttackType.Default_1);
        }
        _attackModule.DefaultAttack(); // 실제적인 공격 수행( 범위 체크 후 타격, 애니메이션 실행 ) 

        // 공격 수행 후 공격상태로 변경 
        ChangeState(typeof(AttackState));
        AttackState attackState = _curState as AttackState;
    }

    /// <summary>
    /// 공격 상태와 다음 공격 설정 ( 애니메이션 끝나는 시점에 지정 ) 
    /// </summary>
    public void ChangeAttackStateType()
    {
        if (_isAttack == true) // 1초간 기본공격 한 상태면 다음 공격으로 이어서 실행 
        {
            if (_attackModule.CurAttackType == AttackType.Default_3) // 3번째 공격이면 딜레이주기
            {
                _curTime = 0;
                StartCoroutine(Delay(_maxCoolTime));
            }
                //다음 공격 실행
                _attackModule.SetCurAttackType(_attackModule.NextAttackType);
        }
        else 
        {
            // default_1 실행 
            _attackModule.SetCurAttackType(AttackType.Default_1);
        }
    }

    private void TackeAttack()
    {
        _attackModule.SetCurAttackType(AttackType.Tackle);
        _attackModule.DefaultAttack();
    }

    // 피격 관련 
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
    /// 공격중 ( 플레이어 입력 차단 )
    /// </summary>
    public void Attacking()
    {
        _inputModule.Attacking(true);
    }

    /// <summary>
    /// 기본 공격이면 입력 차단 + 대쉬 체크
    /// </summary>
    public void DefaultAttacking()
    {
        Attacking();
        _moveModule.StopMove();
        _moveModule.Dash();
    }

    /// <summary>
    /// 공격 끝
    /// </summary>
    public void EndAttacking()
    {
        _inputModule.Attacking(false);
        //ChangeAttackStateType(); 
    }

}
