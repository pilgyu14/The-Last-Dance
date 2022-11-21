using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class TestE
{
    private NavMeshAgent _agent; 
   
    void A()
    {
    }
    
}

// 상하 좌우 이동 
// 회전 
// 액션 
// 애니메이션 



public class PlayerController : MonoBehaviour, IAgent ,IDamagable
{
    // 인스펙터 
    [SerializeField]
    private AgentSO _playerSO;

    // 캐싱 변수 
    private InputModule _inputModule;
    private CharacterController _chController;
    private PlayerAnimation _playerAnimation; 

    // 내부 변수 
    private bool _isDie = false;
    private bool _isBattle = false; // 전투중인가 

    private void Awake()
    {
        _inputModule = GetComponent<InputModule>();
        _chController = GetComponent<CharacterController>();
        _playerAnimation = transform.GetComponentInChildren<PlayerAnimation>();  
    
    }

    private void Update()
    {
        Move(); 
    }

    private void Move()
    {
        _chController.Move(_inputModule.MoveDir * _playerSO.speed * Time.deltaTime);
        if(_chController.velocity .magnitude> 0.2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inputModule.MoveDir, Vector3.up),
            Time.deltaTime * _playerSO.speed);
        }

        _playerAnimation.AnimatePlayer(_chController.velocity.magnitude);
    }

    private void InBattleMove()
    {
        _chController.Move(_inputModule.MoveDir * _playerSO.speed * Time.deltaTime);
        if (_chController.velocity.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inputModule.MoveDir, Vector3.up),
            Time.deltaTime * _playerSO.speed);
        }

        _playerAnimation.SetVelocity(_inputModule.MoveDir.x, _inputModule.MoveDir.y); 
    }

    private float _time = 0f;  // 전투 지속X 시간 
    private void CheckBattle()
    {
        if (_isBattle == false) return; 

        _time += Time.deltaTime;
        if (_isBattle == true && _time >= 5f) // 전투상태가 일정시간 지속되지 않았을때 
        {
            _isBattle = false;
            _time = 0;
            _playerAnimation.SetBattle(_isBattle); 
        }
    }

    private void StartBattle()
    {
        _time = 0;
        _isBattle = true;
        _playerAnimation.SetBattle(_isBattle); 
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
}
