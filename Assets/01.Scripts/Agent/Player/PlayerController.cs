using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// 상하 좌우 이동 
// 회전 
// 액션 
// 애니메이션 

public class PlayerController : MonoBehaviour, IAgent, IDamagable,IKnockback
{
    #region 변수 
    // 인스펙터 
    [SerializeField]
    private PlayerSO _playerSO;
    [SerializeField]
    private SkillInventorySO _skillInventorySO; 
    [SerializeField]
    private UnityEvent feedbackCallbackHit = null; // 스크린 이펙트 
    [SerializeField]
    private FeedbackPlayer _lowHpFeedback; // 체력 낮을 때 피드백 

    [SerializeField]
    private EffectFeedback lowHpFeedback; // 체력 낮을 때 피드백 

    [SerializeField]
    private SkillComponent _skillComponent;
    private SkillSaveComponent _skillSaveComponent;
    private PassiveSkillManageComponent _passiveSkillManageComponent; 
    // 캐싱 변수 
    private InputModule _inputModule;
    private PlayerMoveModule _moveModule;
    private AttackModule _attackModule;
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

    private bool _isDelay = false; // 기본 공격할 수 있는가 
    [SerializeField]
    private float _curTime = 0f;
    private float _maxCoolTime = 0.5f;  // 기본 공격 3타 후 딜레이 

    private bool _isLowHpEffect = false; // hp 적을 때 이펙트 

    // 프로퍼티 
    public PlayerSO PlayerSO => _playerSO; 
    public InputModule InputModule => _inputModule;
    public PlayerMoveModule MoveModule => _moveModule;
    public AttackModule AttackModule => _attackModule;
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



    // 임시 


    // 임시 


    #region 초기화 
    private void Awake()
    {
        _inputModule = GetComponentInChildren<InputModule>();
        _moveModule = GetComponent<PlayerMoveModule>();
        _attackModule = GetComponentInChildren<AttackModule>();
        _fov = GetComponent<FieldOfView>();
        //_chController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _hpModule = GetComponent<HPModule>();
        _autioPlayer = GetComponentInChildren<AgentAudioPlayer>();
        _passiveSkillManageComponent = GetComponentInChildren<PassiveSkillManageComponent>(); 
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

        _inputModule.RegisterKeyAction(KeyCode.Alpha1, HurricaneKickAttack);

        _playerSO.UpdateStat();
        // 모듈 초기화
        _moveModule.Init(this, _agent, _playerSO.moveInfo, _playerAnimation, _inputModule);
        _attackModule.Init(this, _fov, _moveModule,_playerAnimation);
        _hpModule.Init(_playerSO.maxHp, _playerSO.maxHp);

        // 스킬 관려 설정 
        _skillComponent.AddAttackData(AttackType.HurricaneAttack,() => HurricaneKickAttack());

        _skillSaveComponent = new SkillSaveComponent();
        _skillSaveComponent.Init(_inputModule, _skillComponent,_skillInventorySO); 

        _lowHpFeedback.FinishAllFeedbacks();

        SetThreeAttackPossible(false); //삼타 설정 
    
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

    [SerializeField] private KeyCode keyCode; 
    public void CheckActiveSkill()
    {
        EventManager.Instance.TriggerEvent(EventsType.SetActiveSkillInput, keyCode); 
    }
    // 기본 애니메이션 움직임 
    public void MoveDefaultAnimation(Vector3 v)
    {
        _playerAnimation.AnimatePlayer(v.sqrMagnitude);
        //_playerAnimation.AnimatePlayer(_agent.velocity.magnitude);
    }

    #region 공격 관련 
    // 기본 공격 3타 후 딜레이
    IEnumerator Delay(float delayTime)
    {
        Debug.LogError("딜레이 시작");

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
                        _playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            Debug.Log("  공격 안돼요 ");
            return;
        }
        ChangeAttackStateType(); 
        if (_attackModule.NextAttackType == AttackType.Null) // 태클같은 스킬인 경우 ??
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
        if (_isAttack == true) // 두번 째 공격부터야 ,  1초간 기본공격 한 상태면 다음 공격으로 이어서 실행 
        {
              //다음 공격 실행
             _attackModule.SetCurAttackType(_attackModule.NextAttackType);
            
            if (_attackModule.CurAttackType == _attackModule.LastAttackType) // 3번째 공격이면 딜레이주기
            {
                _curTime = 0;
                StartCoroutine(Delay(_maxCoolTime));
            }
        }
        else // 첫 타 
        {
            // default_1 실행 
            _attackModule.SetCurAttackType(AttackType.Default_1);
        }
    }

    /// <summary>
    /// 삼타 공격 설정
    /// </summary>
    /// <param name="isCan"></param>
    public void SetThreeAttackPossible(bool isCan)
    {
        _attackModule.IsThreeAttack = isCan;
        CheckThreeAttackPossible(); 
    }

    /// <summary>
    /// 삼타 공격까지 가능한지 체크 
    /// </summary>
    [ContextMenu("삼타 체크")]
    private void CheckThreeAttackPossible()
    {
        if(_attackModule.IsThreeAttack == true)        // 삼타 공격 가능하면 
        {
            _attackModule.GetAttackInfo(AttackType.Default_2).attackInfo.nextAttackType = AttackType.Default_3;
            _attackModule.LastAttackType = AttackType.Default_3;
            return; 
        }

        _attackModule.GetAttackInfo(AttackType.Default_2).attackInfo.nextAttackType = AttackType.Default_1;
        _attackModule.LastAttackType = AttackType.Default_2;
    }

    // 태클 공격 시 실행 (shift 입력시 ) 
    private void TackeAttack()
    {
        EndBattleState();
        _attackModule.SetCurAttackType(AttackType.Tackle);
        _attackModule.DefaultAttack();
        _playerAnimation.SetBattle(_isBattle);
    }

    private void HurricaneKickAttack()
    {
        EndBattleState();
        _attackModule.SetCurAttackType(AttackType.HurricaneAttack);
        _attackModule.DefaultAttack();
        
        _playerAnimation.SetBattle(_isBattle);
      
        _moveModule.InitCurRotSpeed();
    }

    public void CheckEndHurricaneKick()
    {
        // 한번 더 클릭 이벤트를 받아야해 
        bool isEnd = _attackModule.CurAttackBase.lsEndAttackDuration(); 
        if (isEnd  == true)
        {
            _playerAnimation.PlayHurricaneKick(false); // 애니메이션 끄기 

        }
    }

    #endregion
    // 피격 관련 
    public void OnDie()
    {
        _moveModule.StopMove(); 
        _inputModule.BlockAllInput(true); 
        _isDie = true;
        _playerAnimation.PlayDeathAnimation(); 
        
        // UI 뜨도록 
    }

    public bool IsDie()
    {
        return _isDie;
    }

    // 피격시 호출 
    public void GetDamaged(int damage, GameObject damageDealer)
    {
        if (IsDie() == true) return;
        feedbackCallbackHit?.Invoke(); 
        if (CheckHPEffect(damage) == false)
        {
            OnDie(); // 죽음 처리 
        }
        // 모든 입력 막아야해 
        _moveModule.StopMove(); 
        _inputModule.BlockPlayerInput(true); 
        _playerAnimation.PlayHitAnimation();

        EventManager.Instance.TriggerEvent(EventsType.UpdateHpUI); // UI 업데이트 
    }

    /// <summary>
    /// 체력 적으면 이글이글 효과 
    /// </summary>
    public bool CheckHPEffect(int damage)
    {
        bool isDie = _hpModule.ChangeHP(-damage);
        if( isDie == false && _isLowHpEffect == false &&_hpModule.HP/_hpModule.MaxHp < 0.2f) // 이펙트가 꺼져있으면서 체력 20퍼 미만 이면 
        {
            // 이펙트 시작 1
            _lowHpFeedback.PlayAllFeedbacks();
            _isLowHpEffect = true; 
        }
        else if(_isLowHpEffect == true)
        {
            // 이펙트 꺼주기 
            _lowHpFeedback.FinishAllFeedbacks();
            _isLowHpEffect = false; 
        }
        return isDie; 
    }

    private void Check()
    {

    }


    public void Knockback(Vector3 direction, float power, float duration)
    {
        StartCoroutine(_moveModule.DashCorutine(direction, power, duration)); 
    }

    #region 애니메이션 Behaviour 함수 

    /// <summary>
    /// 피격 끝났으면 입력 가능하도록 
    /// </summary>
    public void EndHit()
    {
        _inputModule.BlockPlayerInput(false);
    }
    /// <summary>
    /// 공격중 ( 플레이어 입력 차단 ) - 공격 시작 애니메이션 
    /// </summary>
    public void Attacking()
    {
        _moveModule.BlockMove(true); 
        //_inputModule.Attacking(true);
    }

    /// <summary>
    /// 기본 공격이면 입력 차단 + 대쉬 체크 - 기본 공격 시작 애니메이션 
    /// </summary>
    public void DefaultAttacking()
    {
        Attacking();
        _moveModule.StopMove();
        _moveModule.Dash();
    }

    /// <summary>
    /// 공격 끝  - 기본 공격 끝 애니메이션 
    /// </summary>
    public void EndAttacking() 
    {
        _moveModule.BlockMove(false); 
        //_inputModule.Attacking(false);
        //ChangeAttackStateType(); 
    }

    #endregion 

    #region FSM
    // 상태 변경 
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

    // 전투중이 끝났을 때 ( 전투 움직임 ) 
    // Default상태 변경 _isBattle = false 
    public void EndBattleState()
    {
        SetState(typeof(DefaultState), ref _isBattle);
    }

    // 공격이 끝났을 때 ( 3타 연계 ) 
    // Default상태 변경 _isBattle = false 
    public void EndAttackState()
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
        _isBattle = true;
        _playerAnimation.SetBattle(_isBattle);
    }

    public void StartAttack()
    {
        _isBattle = true;
        _playerAnimation.SetBattle(_isBattle);
        _isAttack = true;
    }


    #endregion
}
