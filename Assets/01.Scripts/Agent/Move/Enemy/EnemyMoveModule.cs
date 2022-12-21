using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseComponent
{
    private NavMeshAgent _agent; 
    private Transform _target; 

    public void Init(NavMeshAgent agent, Transform target)
    {
        this._agent = agent; 
        this._target = target; 
    }

    public void Chase()
    {
        _agent.SetDestination(_target.position);
    }
}


/// <summary>
/// Idle 상태 기본 회전 
/// </summary>
public class EnemyRotateComponent
{
    private Vector3 _rotateAxis = Vector3.up; // 회전 축  
    private float _rotateSpeed = 30f;
    private float _chagneTime;
    private float _delayTime; 
    private TimerModule _timeModule;

    public Vector3 RotateAxis => _rotateAxis;
    public float RotateSpeed => _rotateSpeed; 
    public TimerModule Timer => _timeModule; 
    public void Init()
    {
         ChangeRotateValue(); 
        _timeModule = new TimerModule(_chagneTime, () => ChangeAxis()); 
    }

    private void Wait()
    {
        _rotateSpeed = 0f;
        _delayTime = Random.Range(0.5f, 1f);
        _timeModule = new TimerModule(_delayTime,()=> ChangeAxis()); 
    }
    private void ChangeAxis()
    {
        // 가만히 있기 
        _timeModule.ChangeMaxTime(Random.Range(1f, 2f));
        ChangeRotateValue();
        _timeModule = new TimerModule(_chagneTime, () => Wait());
    }

    /// <summary>
    /// 회전 축, 속도 변경 
    /// </summary>
    private void ChangeRotateValue()
    {
        _chagneTime = Random.Range(0.8f, 1.5f);
        int r = Random.Range(0, 2);
        _rotateAxis = r == 0 ? Vector3.up : Vector3.down; // 회전축 랜덤 설정 
        float ra = Random.Range(120f, 240f);
        _rotateSpeed = ra;

    }
}

public class EnemyMoveModule : AgentMoveModule
{
    private Enemy _owner; 

    private EnemyRotateComponent _enemyRotateComponent;
    private EnemyChaseComponent _enemyChaseComponent; 

    [SerializeField]
    private Vector3 _originVec;


    public override void Init(params object[] prms)
    {
        // base.Init(objs);

        this._owner = prms[0] as Enemy;
        this._agent = prms[1] as NavMeshAgent;
        this._movementInfo = prms[2] as MovementInfo;

        _enemyRotateComponent = new EnemyRotateComponent();
        _enemyRotateComponent.Init();

        _enemyChaseComponent = new EnemyChaseComponent();
        _enemyChaseComponent.Init(_agent, _owner.Target);

     }

    private void Start()
    {
        _rotTargetPos = transform.eulerAngles; 
        _originVec = _agent.transform.position; 
    }

    protected override void Update()
    {
        // ApplyGravity();
        _enemyRotateComponent.Timer.UpdateSomething(); // 회전 
    }

    #region Action

    /// <summary>
    /// 돌진 
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="power"></param>
    /// <param name="duration"></param>
    public void Rush(Vector3 dir, float power, float duration)
    {
        StartCoroutine(DashCorutine(dir, power, duration));
    }

    /// <summary>
    ///  기존 위치로 이동 
    /// </summary>
    public void MoveOriginPos()
    {
     //   _agent.SetDestination(_originVec);
    }

    /// <summary>
    /// Idle 상태 이동 ( 제자리에서 회전  반복 ) 
    /// </summary>
    public void RotateIdle()
    {
        transform.Rotate(_enemyRotateComponent.RotateAxis, _enemyRotateComponent.RotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 추적
    /// </summary>
    public void Chase()
    {
        _enemyChaseComponent.Chase(); 
    }
    #endregion

    #region Condition
    /// <summary>
    /// 현재 기본 위치에 있는가 
    /// </summary>
    public bool IsOriginPos()
    {
        //Debug.Log("@@처음위치에서부터 거리 " + (_agent.transform.position - _originVec).sqrMagnitude); 
        return ((_agent.transform.position - _originVec).sqrMagnitude < 3f);
    }
    #endregion

    public void SetOriginPos()
    {
        _originVec = _agent.transform.position;
    }
    //private 
    // 특정 각도로 회전 
    // 특정 위치로 이동 

    //public 
    // 기본 위치로 이동
}

