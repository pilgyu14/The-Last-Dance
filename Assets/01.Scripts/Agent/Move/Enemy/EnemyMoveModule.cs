using UnityEngine; 

public class EnemyMoveModule : AgentMoveModule<Enemy>
{
    private Vector3 _originVec;

    private void Start()
    {
        _rotTargetPos = transform.eulerAngles; 
        _originVec = transform.position; 
    }

    protected override void Update()
    {
        ApplyGravity();
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, _rotTargetPos, Time.deltaTime * 2f);
    }

    public void RotateIdle()
    {
        float l = (_rotTargetPos - transform.eulerAngles).sqrMagnitude; 
        if (l < 0.01f) // 목표 달성 
        {
            SetRotateValue(Random.Range(-4, 5) * 45); 
        }
    }
    private void SetRotateValue(float angle)
    {
        _rotTargetPos = transform.eulerAngles + Vector3.up * angle;

    }

    /// <summary>
    /// 현재 기본 위치에 있는가 
    /// </summary>
    public bool IsOriginPos()
    {
        return ((_agent.transform.position - _originVec).sqrMagnitude < 0.01f);
    }
    /// <summary>
    ///  기존 위치로 이동 
    /// </summary>
    public void MoveOriginPos()
    {
        _agent.SetDestination(_originVec);
    }

    //private 
    // 특정 각도로 회전 
    // 특정 위치로 이동 

    //public 
    // 기본 위치로 이동
}

