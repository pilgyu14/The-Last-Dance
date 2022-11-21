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
    private AgentSO _playerSO;

    // ĳ�� ���� 
    private InputModule _inputModule;
    private CharacterController _chController;
    private PlayerAnimation _playerAnimation; 

    // ���� ���� 
    private bool _isDie = false; 

    // ������Ƽ
    public bool IsDie => _isDie; 
    public float HP => _playerSO.hp; 
    
    private void Awake()
    {
        _inputModule = GetComponent<InputModule>();
        _chController = GetComponent<CharacterController>();
        _playerAnimation = GetComponent<PlayerAnimation>(); 
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
    }

    public void Damaged()
    {
    }

    public void OnDie()
    {
    }
}
