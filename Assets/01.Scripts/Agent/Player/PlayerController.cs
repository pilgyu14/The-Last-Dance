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
        owner.InputModule.OnMovementKeyPress += owner.MoveDefaultAnimation; 
        // move 넣기 
    }
    public override void Stay()
    {

    }

    public override void Exit()
    {
        owner.InputModule.OnMovementKeyPress -= owner.MoveModule.Move;
        owner.InputModule.OnMovementKeyPress -= owner.MoveDefaultAnimation;
    }

}

public class InBattleState : State<PlayerController>
{

    public override void Enter()
    {
        owner.StartBattle();
        owner.InputModule.OnMovementKeyPress += owner.MoveModule.InBattleMove;
        owner.InputModule.OnMovementKeyPress += owner.InBattleMoveAnimation; 
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
        owner.InputModule.OnMovementKeyPress -= owner.InBattleMoveAnimation;
    }



}
#endregion
public class PlayerController : MonoBehaviour, IAgent ,IDamagable
{
    // 인스펙터 
    [SerializeField]
    private AgentSO _playerSO;

    // 캐싱 변수 
    private InputModule _inputModule;
    private MoveModule _moveModule;
    private AttackModule _attackModule;
    private FieldOfView _fov; 
    private CharacterController _chController;
    private NavMeshAgent _agent;
    private PlayerAnimation _playerAnimation;

    // 내부 변수 
    #region State
    private Dictionary<Type, State<PlayerController>> _stateDic = new Dictionary<Type, State<PlayerController>>(); 

    private DefaultState _defaultState;
    private InBattleState _inBattleState;

    private State<PlayerController> _curState;
    private State<PlayerController> _prevState;

    public State<PlayerController> CurState => _curState; 
    #endregion 

    private Vector3 _targetPos;
    private Quaternion _targetRot; 

    private bool _isDie = false;
    [SerializeField]
    private bool _isBattle = false; // 전투상태인가 
    private bool _isAttack = false; // 공격 중인가 

    // 프로퍼티 
    public InputModule InputModule => _inputModule; 
    public MoveModule MoveModule => _moveModule;
    public PlayerAnimation PlayerAnimation => _playerAnimation;

    public bool IsEnemy => false; 

    public Vector3 HitPoint => throw new NotImplementedException();

    private void Awake()
    {
        _inputModule = GetComponent<InputModule>();
        _moveModule = GetComponent<MoveModule>();
        _attackModule = GetComponent<AttackModule>();
        _fov = GetComponent<FieldOfView>(); 
        _chController = GetComponent<CharacterController>();
        //_agent = GetComponent<NavMeshAgent>(); 
        _playerAnimation = transform.GetComponentInChildren<PlayerAnimation>();  
    }

    private void Start()
    {
        // 상태 등록 
        _defaultState = new DefaultState();
        _inBattleState = new InBattleState();

        _defaultState.Init(this);
        _inBattleState.Init(this); 

        _stateDic.Add(_defaultState.GetType(), _defaultState);
        _stateDic.Add(_inBattleState.GetType(), _inBattleState);

        ChangeState(typeof(DefaultState));

        // 입력 등록 
        _inputModule.OnDefaultAttackPress = DefaultKickAttack;

        //_inputModule.OnPointerRotate = RotateByMouse;
        //_inputModule.OnMovementKeyPress = Move;
        // _inputModule.OnMovementKeyPress = InBattleMove; 

        _moveModule.Init(_inputModule,_chController ,_playerSO.moveInfo,_playerAnimation);
        _attackModule.Init(_fov,_moveModule, _playerAnimation);
    }
    private void Update()
    {
        //Debug.Log(_curState.GetType().Name); 
        if (_isAttack == true) return;

        _curState.Stay(); 

        //if (_isBattle == false)
        //{
        //    Move();
        //    return; 
        //}
        //InBattleMove(); 
    }



    public void MoveDefaultAnimation(Vector3 v)
    {
            _playerAnimation.AnimatePlayer(_chController.velocity.magnitude);
        //_playerAnimation.AnimatePlayer(_agent.velocity.magnitude);

    }
    public void InBattleMoveAnimation(Vector3 targetDir)
    {
       // _playerAnimation.SetVelocity(targetDir.x, targetDir.z);
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

            ChangeState(typeof(DefaultState));
        }

    }

    public void StartBattle()
    {
        _time = 0;
        _isBattle = true;
        _playerAnimation.SetBattle(_isBattle);

    }

        
    public void ChangeState(Type type)
    {
        if(_stateDic.ContainsKey(type) == false)
        {
            Debug.LogError("존재하지 않는 상태");
            return;
        }
        
        if(_curState != null)
        {
            _prevState = _stateDic[_curState.GetType()];
            _prevState.Exit();
        }

        _curState = _stateDic[type];
        _curState.Enter(); 
    }

    private void DefaultKickAttack()
    {
        ChangeState(typeof(InBattleState));

        _attackModule.DefaultAttack();


        //switch (_attackModule.curAttackType)
        //{
        //    case AttackType.Default_1:
        //        _playerAnimation.SetFrontKick();
        //        break;
        //    case AttackType.Default_2:
        //        _playerAnimation.SetSideKick();
        //        break;
        //    case AttackType.Default_3:
        //        _playerAnimation.SetBackKick();
        //        break;
        //}

    }

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
    /// 기본 공격이면 입력 차단 + 대쉬 
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
    }


    //private void RotateByMouse(Vector3 pos)
    //{
    //    Ray ray = Define.MainCam.ScreenPointToRay(pos);
    //    Physics.Raycast(ray, out RaycastHit hitInfo);
    //    Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 3f);

    //    _targetPos = hitInfo.point - transform.position;
    //    _targetPos.y = 0;

    //    _targetRot = Quaternion.LookRotation(_targetPos, Vector3.up);

    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up),
    //       Time.deltaTime * _playerSO.speed);

    //    //_targetRot.x = 0;
    //    //_targetRot.z = 0;
    //}

    //private void Move(Vector3 moveDir)
    //{
    //    _chController.Move(moveDir * _playerSO.speed * Time.deltaTime);
    //    if (_chController.velocity.magnitude > 0.2f)
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up),
    //        Time.deltaTime * _playerSO.speed);

    //        _targetPos = moveDir;
    //    }

    //    _playerAnimation.AnimatePlayer(_chController.velocity.magnitude);
    //}

    //private void InBattleMove(Vector3 moveDir)
    //{
    //    Vector3 targetDir = Vector3.Normalize(moveDir.x * transform.right + _inputModule.MoveDir.z * transform.forward); // 회전 값에 따른 이동 방향

    //    _chController.Move(moveDir * _playerSO.speed * Time.deltaTime);
    //    //if (_chController.velocity.magnitude > 0.2f)
    //    //{
    //    transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot,
    //    Time.deltaTime * _playerSO.speed);
    //    //}

    //    _playerAnimation.SetVelocity(targetDir.x, targetDir.z);

    //    CheckBattle();
    //}

}
