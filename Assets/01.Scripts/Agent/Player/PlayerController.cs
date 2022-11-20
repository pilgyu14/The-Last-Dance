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



public class PlayerController : MonoBehaviour
{
    // 캐싱 변수 
    private InputModule _inputModule;
    private CharacterController _chController;
    private PlayerAnimation _playerAnimation; 


    private float _speed = 10;

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
        _chController.Move(_inputModule.MoveDir * _speed * Time.deltaTime);
        if(_chController.velocity .magnitude> 0.2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inputModule.MoveDir, Vector3.up),
            Time.deltaTime * _speed);
        }

  
    }

}
