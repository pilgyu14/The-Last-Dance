using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// ���� �¿� �̵� 
// ȸ�� 
// �׼� 
// �ִϸ��̼� 

public class PlayerController : MonoBehaviour, IAgent, IDamagable,IKnockback
{
    #region ���� 
    // �ν����� 
    [SerializeField]
    private PlayerSO _playerSO;
    [SerializeField]
    private SkillInventorySO _skillInventorySO; 
    [SerializeField]
    private UnityEvent feedbackCallbackHit = null; // ��ũ�� ����Ʈ 
    [SerializeField]
    private FeedbackPlayer _lowHpFeedback; // ü�� ���� �� �ǵ�� 

    [SerializeField]
    private EffectFeedback lowHpFeedback; // ü�� ���� �� �ǵ�� 

    [SerializeField]
    private SkillComponent _skillComponent;
    private SkillSaveComponent _skillSaveComponent;
    private PassiveSkillManageComponent _passiveSkillManageComponent; 
    // ĳ�� ���� 
    private InputModule _inputModule;
    private PlayerMoveModule _moveModule;
    private AttackModule _attackModule;
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

    private bool _isDelay = false; // �⺻ ������ �� �ִ°� 
    [SerializeField]
    private float _curTime = 0f;
    private float _maxCoolTime = 0.5f;  // �⺻ ���� 3Ÿ �� ������ 

    private bool _isLowHpEffect = false; // hp ���� �� ����Ʈ 

    // ������Ƽ 
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



    // �ӽ� 


    // �ӽ� 


    #region �ʱ�ȭ 
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

        _inputModule.RegisterKeyAction(KeyCode.Alpha1, HurricaneKickAttack);

        _playerSO.UpdateStat();
        // ��� �ʱ�ȭ
        _moveModule.Init(this, _agent, _playerSO.moveInfo, _playerAnimation, _inputModule);
        _attackModule.Init(this, _fov, _moveModule,_playerAnimation);
        _hpModule.Init(_playerSO.maxHp, _playerSO.maxHp);

        // ��ų ���� ���� 
        _skillComponent.AddAttackData(AttackType.HurricaneAttack,() => HurricaneKickAttack());

        _skillSaveComponent = new SkillSaveComponent();
        _skillSaveComponent.Init(_inputModule, _skillComponent,_skillInventorySO); 

        _lowHpFeedback.FinishAllFeedbacks();

        SetThreeAttackPossible(false); //��Ÿ ���� 
    
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
    // �⺻ �ִϸ��̼� ������ 
    public void MoveDefaultAnimation(Vector3 v)
    {
        _playerAnimation.AnimatePlayer(v.sqrMagnitude);
        //_playerAnimation.AnimatePlayer(_agent.velocity.magnitude);
    }

    #region ���� ���� 
    // �⺻ ���� 3Ÿ �� ������
    IEnumerator Delay(float delayTime)
    {
        Debug.LogError("������ ����");

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
                        _playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            Debug.Log("  ���� �ȵſ� ");
            return;
        }
        ChangeAttackStateType(); 
        if (_attackModule.NextAttackType == AttackType.Null) // ��Ŭ���� ��ų�� ��� ??
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
        if (_isAttack == true) // �ι� ° ���ݺ��;� ,  1�ʰ� �⺻���� �� ���¸� ���� �������� �̾ ���� 
        {
              //���� ���� ����
             _attackModule.SetCurAttackType(_attackModule.NextAttackType);
            
            if (_attackModule.CurAttackType == _attackModule.LastAttackType) // 3��° �����̸� �������ֱ�
            {
                _curTime = 0;
                StartCoroutine(Delay(_maxCoolTime));
            }
        }
        else // ù Ÿ 
        {
            // default_1 ���� 
            _attackModule.SetCurAttackType(AttackType.Default_1);
        }
    }

    /// <summary>
    /// ��Ÿ ���� ����
    /// </summary>
    /// <param name="isCan"></param>
    public void SetThreeAttackPossible(bool isCan)
    {
        _attackModule.IsThreeAttack = isCan;
        CheckThreeAttackPossible(); 
    }

    /// <summary>
    /// ��Ÿ ���ݱ��� �������� üũ 
    /// </summary>
    [ContextMenu("��Ÿ üũ")]
    private void CheckThreeAttackPossible()
    {
        if(_attackModule.IsThreeAttack == true)        // ��Ÿ ���� �����ϸ� 
        {
            _attackModule.GetAttackInfo(AttackType.Default_2).attackInfo.nextAttackType = AttackType.Default_3;
            _attackModule.LastAttackType = AttackType.Default_3;
            return; 
        }

        _attackModule.GetAttackInfo(AttackType.Default_2).attackInfo.nextAttackType = AttackType.Default_1;
        _attackModule.LastAttackType = AttackType.Default_2;
    }

    // ��Ŭ ���� �� ���� (shift �Է½� ) 
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
        // �ѹ� �� Ŭ�� �̺�Ʈ�� �޾ƾ��� 
        bool isEnd = _attackModule.CurAttackBase.lsEndAttackDuration(); 
        if (isEnd  == true)
        {
            _playerAnimation.PlayHurricaneKick(false); // �ִϸ��̼� ���� 

        }
    }

    #endregion
    // �ǰ� ���� 
    public void OnDie()
    {
        _moveModule.StopMove(); 
        _inputModule.BlockAllInput(true); 
        _isDie = true;
        _playerAnimation.PlayDeathAnimation(); 
        
        // UI �ߵ��� 
    }

    public bool IsDie()
    {
        return _isDie;
    }

    // �ǰݽ� ȣ�� 
    public void GetDamaged(int damage, GameObject damageDealer)
    {
        if (IsDie() == true) return;
        feedbackCallbackHit?.Invoke(); 
        if (CheckHPEffect(damage) == false)
        {
            OnDie(); // ���� ó�� 
        }
        // ��� �Է� ���ƾ��� 
        _moveModule.StopMove(); 
        _inputModule.BlockPlayerInput(true); 
        _playerAnimation.PlayHitAnimation();

        EventManager.Instance.TriggerEvent(EventsType.UpdateHpUI); // UI ������Ʈ 
    }

    /// <summary>
    /// ü�� ������ �̱��̱� ȿ�� 
    /// </summary>
    public bool CheckHPEffect(int damage)
    {
        bool isDie = _hpModule.ChangeHP(-damage);
        if( isDie == false && _isLowHpEffect == false &&_hpModule.HP/_hpModule.MaxHp < 0.2f) // ����Ʈ�� ���������鼭 ü�� 20�� �̸� �̸� 
        {
            // ����Ʈ ���� 1
            _lowHpFeedback.PlayAllFeedbacks();
            _isLowHpEffect = true; 
        }
        else if(_isLowHpEffect == true)
        {
            // ����Ʈ ���ֱ� 
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

    #region �ִϸ��̼� Behaviour �Լ� 

    /// <summary>
    /// �ǰ� �������� �Է� �����ϵ��� 
    /// </summary>
    public void EndHit()
    {
        _inputModule.BlockPlayerInput(false);
    }
    /// <summary>
    /// ������ ( �÷��̾� �Է� ���� ) - ���� ���� �ִϸ��̼� 
    /// </summary>
    public void Attacking()
    {
        _moveModule.BlockMove(true); 
        //_inputModule.Attacking(true);
    }

    /// <summary>
    /// �⺻ �����̸� �Է� ���� + �뽬 üũ - �⺻ ���� ���� �ִϸ��̼� 
    /// </summary>
    public void DefaultAttacking()
    {
        Attacking();
        _moveModule.StopMove();
        _moveModule.Dash();
    }

    /// <summary>
    /// ���� ��  - �⺻ ���� �� �ִϸ��̼� 
    /// </summary>
    public void EndAttacking() 
    {
        _moveModule.BlockMove(false); 
        //_inputModule.Attacking(false);
        //ChangeAttackStateType(); 
    }

    #endregion 

    #region FSM
    // ���� ���� 
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

    // �������� ������ �� ( ���� ������ ) 
    // Default���� ���� _isBattle = false 
    public void EndBattleState()
    {
        SetState(typeof(DefaultState), ref _isBattle);
    }

    // ������ ������ �� ( 3Ÿ ���� ) 
    // Default���� ���� _isBattle = false 
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
