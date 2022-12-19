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

    private bool _isInput = true; 
    private bool _isPlayerInput = true;
    private bool _isUIInput = true;


    // ���� 
    private float _x;
    private float _y;
    [SerializeField]
    private Vector3 _moveDir;

    // Ʈ���� 
    public Action<Vector3> OnMovementKeyPress { get; set; } // ������ 
    public Action<Vector3> OnMoveAnimation { get; set; } // ������ �ִϸ��̼� 
    public Action<Vector3> OnPointerRotate { get ; set; } // ���콺 ȸ�� 
    public Action OnDefaultAttackPress { get; set ; } // �⺻ ���� 

    public Action<Vector3> OnKeyboardRotate = null;
    public Action OnShift = null; 

    // ������Ƽ 
    public Vector3 MoveDir => _moveDir; 
    public bool IsPlayerInput => _isPlayerInput;
    public bool IsUIInputInput => _isUIInput;

    public void Init(PlayerController playerController)
    {
        this._playerController = playerController; 
    }

    private void Update()
    {
        if (_isInput == false) return; 

        if (_isPlayerInput == true)
        {
            PlayerInput(); 
        }
            
        if(_isUIInput == true)
        {   
            UIInput(); 
        }
    
    }


    /// <summary>
    /// ��� �Է� ���� ( �׾��� �� )
    /// </summary>
    /// <param name="isBlock"></param>
    public void BlockAllInput(bool isBlock)
    {
        _isInput = ! isBlock;
    }

    /// <summary>
    /// �÷��̾� �Է� ����( UI �Է� �� ) 
    /// </summary>
    /// <param name="isBlock"></param>
    public void BlockPlayerInput(bool isBlock)
    {
        _isPlayerInput = !isBlock; 
    }



    /// <summary>
    /// �������̸� �Է� ���� 
    /// </summary>
    /// <param name="isAttacking"></param>
    public void Attacking(bool isAttacking)
    {
        if(isAttacking)
        {
            _isMove = false;
           // _isAttackInput = false; 
        } 
        else
        {
            _isMove = true;
          // _isAttackInput = true;
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
            _moveDir = Define.MainCam.transform.TransformDirection(_moveDir);

            OnMoveAnimation?.Invoke(MoveDir);

            if (_isMove == true)
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

            // ��Ŭ
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                OnShift?.Invoke();
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
            RotateRay();
            //OnPointerRotate?.Invoke(Define.WorldMousePos);
        }
        else // ���� ���¸� 
        {
            RotateRay(); 
        }
    }

    /// <summary>
    /// ���̽��� �������� ȸ��( ���̿� �浹������ �������� ���� ���) 
    /// </summary>
    private void RotateRay()
    {
        if (Physics.Raycast(Define.MainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            OnPointerRotate?.Invoke(hitInfo.point);
        }
        else // �ϴ��� ������� 
        {
            Vector3 originCameraPosition = Define.MainCam.transform.position;
            Vector3 dir = Define.MainCam.ScreenPointToRay(Input.mousePosition).direction;
            Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);

            Vector3 clickingPosition = ray.origin + ray.direction / ray.direction.y * (0 - originCameraPosition.y);
            OnPointerRotate?.Invoke(clickingPosition);
        }
    }
    

}
