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
    private TimerModule _timeModule;

    public Vector3 RotateAxis => _rotateAxis;
    public TimerModule Timer => _timeModule; 
    public void Init()
    {
        ChangeRotateValue(); 
        _timeModule = new TimerModule(1f, () => ChangeAxis()); 
    }

    private void ChangeAxis()
    {
        _timeModule.ChangeMaxTime(Random.Range(2f, 6f));
        ChangeRotateValue();
    }

    /// <summary>
    /// 회전 축, 속도 변경 
    /// </summary>
    private void ChangeRotateValue()
    {
        int r = Random.Range(0, 2);
        _rotateAxis = r == 0 ? Vector3.up : Vector3.down; // 회전축 랜덤 설정 
        float ra = Random.Range(30f, 80f);
        _rotateSpeed = ra;

        Debug.Log("@@@" + _rotateAxis);
        Debug.Log("@@@" + _rotateSpeed);
    }
}

public class EnemyMoveModule : AgentMoveModule<Enemy>
{
    private EnemyRotateComponent _enemyRotateComponent;
    private EnemyChaseComponent _enemyChaseComponent; 

    private Vector3 _originVec;


    public override void Init(params object[] prms)
    {
        // base.Init(objs);

        this.owner = prms[0] as Enemy;
        this._agent = prms[1] as NavMeshAgent;
        this._movementInfo = prms[2] as MovementInfo;

        _enemyRotateComponent = new EnemyRotateComponent();
        _enemyRotateComponent.Init();

        _enemyChaseComponent = new EnemyChaseComponent();
        _enemyChaseComponent.Init(_agent, owner.Target);

     }

    private void Start()
    {
        _rotTargetPos = transform.eulerAngles; 
        _originVec = transform.position; 
    }

    protected override void Update()
    {
        // ApplyGravity();
        _enemyRotateComponent.Timer.UpdateSomething(); 
    }

    #region Action

    /// <summary>
    ///  기존 위치로 이동 
    /// </summary>
    public void MoveOriginPos()
    {
        _agent.SetDestination(_originVec);
    }

    /// <summary>
    /// Idle 상태 이동 ( 제자리에서 회전  반복 ) 
    /// </summary>
    public void RotateIdle()
    {
        transform.Rotate(_enemyRotateComponent.RotateAxis, 30f * Time.deltaTime);
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
        return ((_agent.transform.position - _originVec).sqrMagnitude < 0.01f);
    }
    #endregion

    //private 
    // 특정 각도로 회전 
    // 특정 위치로 이동 

    //public 
    // 기본 위치로 이동
}

