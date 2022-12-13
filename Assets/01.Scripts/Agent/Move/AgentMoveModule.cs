using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AgentMoveModule<T> : MonoBehaviour,IComponent where T : IDamagable
{
    protected T owner;


    //  캐싱 변수 
    protected AgentAnimation _agentAnimation;
    protected NavMeshAgent _agent;
    protected MovementInfo _movementInfo;

    [SerializeField] // 임시 직렬화 
    protected float _curSpeed; // 현재 속도 
    protected Vector3 _rotTargetPos; // 목표 회전 vector
    protected Quaternion _targetRot;// 목표 회전 quaternion
    protected Vector3 _targetDir;

    [SerializeField]
    protected float _velocityY = 0f;

    [SerializeField]
    protected LayerMask _layerMask;
    // 이동 회전 

    // 체크 변수 

    // 프로퍼티 
    public virtual Vector3 MoveDir { get;  }

    public virtual void Init(params object[] objs) { }

    //public void Init(NavMeshAgent nAgent, MovementInfo moveInfo, PlayerAnimation playerAnimation, InputModule inputModule = null)
    //{
    //    this._movementInfo = moveInfo;
    //    this._agent = nAgent;
    //    this._playerAnimation = playerAnimation;
    //}

    protected virtual void Update()
    {
        ApplyGravity();
        _agent.Move((_targetDir + Vector3.up * _velocityY) * Time.deltaTime);
    }


    /// <summary>
    /// 기본 상태일때 회전 (이동 방향으로)
    /// </summary>
    /// <param name="moveDir"></param>
    public void RotateDefault(Vector3 moveDir)
    {
        // if (_agent.velocity.magnitude > 0.2f)
        if (moveDir.sqrMagnitude > 0.1f) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up),
            Time.deltaTime * _curSpeed);

            _rotTargetPos = moveDir;
        }
    }

    /// <summary>
    /// 가속 감속 체크 
    /// </summary>
    protected void CheckInput(Vector3 moveDir)
    {
        // 가속 감속 체크 
        if (moveDir.sqrMagnitude > 0) // 입력(이동) 중이면
        {
            _curSpeed += _movementInfo.acceleration * Time.deltaTime;
        }
        else
        {
            _curSpeed -= _movementInfo.deacceleration * Time.deltaTime;
        }
        _curSpeed = Mathf.Clamp(_curSpeed, 0, _movementInfo.maxSpeed);
    }

    /// <summary>
    /// 기본 상태 이동 ㅌ
    /// </summary>
    /// <param name="moveDir"></param>
    public virtual void Move(Vector3 moveDir)
    {
        //Debug.Log("무브");
        CheckInput(moveDir);
        _targetDir = moveDir * _curSpeed;
    }

    /// <summary>
    /// 중력 적용 
    /// </summary>
    protected void ApplyGravity()
    {
       // Debug.Log(_agent.transform.position);
        //Ray ray = new Ray(transform.position - Vector3.up * 0.2f, - Vector3.down);
        Ray ray = new Ray(_agent.transform.position + Vector3.up * (- _agent.baseOffset) , Vector3.down);
        float distance = _agent.baseOffset + 0.1f;
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, distance);

        // 바닥이 땅이면 중력 적용X 
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, _layerMask) == true)
        {
            _velocityY = 0;
        }
        else
        {
            _velocityY += -9.8f * Time.deltaTime;
            transform.Translate(Vector3.up * _velocityY * Time.deltaTime);
        }
    }

    /// <summary>
    /// 공격시 대쉬 
    /// </summary>
    public void Dash()
    {
        if (Vector3.Dot(transform.forward, MoveDir) > 0 || owner.IsEnemy == true)
        {
            Debug.Log("대쉬!");
            StartCoroutine(DashCorutine(transform.forward.normalized, 10f, 0.1f));
        }
    }

    public IEnumerator DashCorutine(Vector3 direction, float power, float duration)
    {
        direction.y = 0;
        _targetDir = direction * power;
        yield return new WaitForSeconds(duration);
        StopMove();
    }

    public void StopDash()
    {

    }

    /// <summary>
    ///  움직임 멈춤
    /// </summary>
    public virtual void StopMove()
    {
        _targetDir = Vector3.zero;
        _agent.isStopped = true;
        _agent.ResetPath(); 
    }
}
