using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputBinding
{
    
}


public class InputModule : MonoBehaviour, IAgentInput
{
    private PlayerController _playerController; 

    // 체크 변수 
    private bool _isMoveInput = true;
    private bool _isMove = true; 

    private bool _isAttackInput = true;


    private bool _isPlayerInput = true;
    private bool _isUIInput = true;


    // 변수 
    private float _x;
    private float _y;
    [SerializeField]
    private Vector3 _moveDir;

    // 트리거 
    public Action<Vector3> OnMovementKeyPress { get; set; } // 움직임 
    public Action<Vector3> OnPointerRotate { get ; set; } // 마우스 회전 
    public Action OnDefaultAttackPress { get; set ; } // 기본 공격 

    public Action<Vector3> OnKeyboardRotate = null;

    // 프로퍼티 
    public Vector3 MoveDir => _moveDir; 
    public bool IsPlayerInput => _isPlayerInput;
    public bool IsUIInputInput => _isUIInput;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>(); 
    }

    private void Update()
    {
        if(_isPlayerInput == true)
        {
            PlayerInput(); 
        }

        if(_isUIInput == true)
        {   
            UIInput(); 
        }
    
    }

    public void Attacking(bool isAttacking)
    {
        if(isAttacking)
        {
            _isMove = false;
            _isAttackInput = false; 
        }
        else
        {
            _isMove = true;
            _isAttackInput = true;
        }
    }
    /// <summary>
    /// Player 입력 받기 
    /// </summary>

    private void PlayerInput()
    {
        if(_isMoveInput == true)
        {
            // x y 입력 
            _x = Input.GetAxisRaw("Horizontal");
            _y = Input.GetAxisRaw("Vertical");
            _moveDir = new Vector3(_x, 0, _y).normalized;

            if(_isMove == true)
            {
                // 이동 
                OnMovementKeyPress?.Invoke(MoveDir);

                // 회전
                CheckRotate();
            }
        }

        if(_isAttackInput == true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                OnDefaultAttackPress?.Invoke();
            }
        }
    }   

    /// <summary>
    /// UI 입력 받기 
    /// </summary>

    private void UIInput()
    {

    }

    /// <summary>
    /// 기본 상태, 전투 상태 이동 
    /// </summary>
    private void CheckRotate()
    {
        if(_playerController.CurState.GetType() == typeof(DefaultState))
        {
            if (MoveDir.sqrMagnitude > 0) // 키보드 입력 중이면 
            {
                OnKeyboardRotate?.Invoke(MoveDir);
                return;
            }
            //Debug.Log("마우스 회전"); 
            //아니면 마우스 회전 
            OnPointerRotate?.Invoke(Define.WorldMousePos);
        }
        else // 전투 상태면 
        {
            OnPointerRotate?.Invoke(Define.WorldMousePos);
        }
    }
    

}
