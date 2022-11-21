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

// ���� �¿� �̵� 
// ȸ�� 
// �׼� 
// �ִϸ��̼� 



public class PlayerController : MonoBehaviour, IAgent ,IDamagable
{
    // �ν����� 
    [SerializeField]
    private AgentSO _playerSO;

    // ĳ�� ���� 
    private InputModule _inputModule;
    private CharacterController _chController;
    private PlayerAnimation _playerAnimation; 

    // ���� ���� 
    private bool _isDie = false;
    private bool _isBattle = false; // �������ΰ� 

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

    private float _time = 0f;  // ���� ����X �ð� 
    private void CheckBattle()
    {
        if (_isBattle == false) return; 

        _time += Time.deltaTime;
        if (_isBattle == true && _time >= 5f) // �������°� �����ð� ���ӵ��� �ʾ����� 
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
