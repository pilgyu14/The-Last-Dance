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
    // 체크 변수 
    private bool _isPlayerInput = true;
    private bool _isMoveInput = true;
    private bool _isAttackInput = true; 
    private bool _isUIInput = true;


    // 변수 
    private float _x;
    private float _y;
    private Vector3 _moveDir;

    // 트리거 
    public Action<Vector3> OnMovementKeyPress { get; set; } // 움직임 
    public Action<Vector3> OnPointerRotate { get ; set; } // 회전 
    public Action OnDefaultAttackPress { get; set ; } // 기본 공격 

    // 프로퍼티 
    public Vector3 MoveDir => _moveDir; 
    public bool IsPlayerInput => _isPlayerInput;
    public bool IsUIInputInput => _isUIInput;

   

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

    /// <summary>
    /// Player 입력 받기 
    /// </summary>

    private void PlayerInput()
    {
        if(_isMoveInput == true)
        {
            _x = Input.GetAxisRaw("Horizontal");
            _y = Input.GetAxisRaw("Vertical");

            _moveDir = new Vector3(_x, 0, _y).normalized;

            OnMovementKeyPress?.Invoke(MoveDir); 

            OnPointerRotate?.Invoke(Input.mousePosition);
        }

        if(_isAttackInput == true)
        {
            if (Input.GetMouseButtonDown(0))
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
}
