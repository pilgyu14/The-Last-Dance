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

    // üũ ���� 
    private bool _isMoveInput = true;
    private bool _isMove = true; 

    private bool _isAttackInput = true;


    private bool _isPlayerInput = true;
    private bool _isUIInput = true;


    // ���� 
    private float _x;
    private float _y;
    [SerializeField]
    private Vector3 _moveDir;

    // Ʈ���� 
    public Action<Vector3> OnMovementKeyPress { get; set; } // ������ 
    public Action<Vector3> OnPointerRotate { get ; set; } // ���콺 ȸ�� 
    public Action OnDefaultAttackPress { get; set ; } // �⺻ ���� 

    public Action<Vector3> OnKeyboardRotate = null;

    // ������Ƽ 
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
    /// Player �Է� �ޱ� 
    /// </summary>

    private void PlayerInput()
    {
        if(_isMoveInput == true)
        {
            // x y �Է� 
            _x = Input.GetAxisRaw("Horizontal");
            _y = Input.GetAxisRaw("Vertical");
            _moveDir = new Vector3(_x, 0, _y).normalized;

            if(_isMove == true)
            {
                // �̵� 
                OnMovementKeyPress?.Invoke(MoveDir);

                // ȸ��
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
    /// UI �Է� �ޱ� 
    /// </summary>

    private void UIInput()
    {

    }

    /// <summary>
    /// �⺻ ����, ���� ���� �̵� 
    /// </summary>
    private void CheckRotate()
    {
        if(_playerController.CurState.GetType() == typeof(DefaultState))
        {
            if (MoveDir.sqrMagnitude > 0) // Ű���� �Է� ���̸� 
            {
                OnKeyboardRotate?.Invoke(MoveDir);
                return;
            }
            //Debug.Log("���콺 ȸ��"); 
            //�ƴϸ� ���콺 ȸ�� 
            OnPointerRotate?.Invoke(Define.WorldMousePos);
        }
        else // ���� ���¸� 
        {
            OnPointerRotate?.Invoke(Define.WorldMousePos);
        }
    }
    

}
