using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class InputBinding
{
    public KeyCode keyCode;
    public Action callback = null; 

    public InputBinding(KeyCode keyCode, Action callback)
    {
        this.keyCode = keyCode;
        this.callback = callback; 
    }
}


public class InputModule : MonoBehaviour, IAgentInput
{
    private PlayerController _playerController; 

    // 체크 변수 
    [SerializeField]
    private bool _isMoveInput = true;
    private bool _isMove = true;
    private bool _isRotate = true; 
    [SerializeField]
    private bool _isAttackInput = true;
    
    [SerializeField]
    private bool _isInput = true; 
    private bool _isPlayerInput = true;
    private bool _isUIInput = true;


    // 변수 
    private float _x;
    private float _y;
    [SerializeField]
    private Vector3 _moveDir;

    // 트리거 
    public List<InputBinding> skillInputList = new List<InputBinding>(); // 1 2 3 4 

    public Action<Vector3> OnMovementKeyPress { get; set; } // 움직임 
    public Action<Vector3> OnMoveAnimation { get; set; } // 움직임 애니메이션 
    public Action<Vector3> OnPointerRotate { get ; set; } // 마우스 회전 
    public Action OnDefaultAttackPress { get; set ; } // 기본 공격 

    public Action<Vector3> OnKeyboardRotate = null;
    public Action OnShift = null; 

    // 프로퍼티 
    public Vector3 MoveDir => _moveDir; 
    public bool IsPlayerInput => _isPlayerInput;
    public bool IsUIInputInput => _isUIInput;

    public void Init(PlayerController playerController)
    {
        this._playerController = playerController;

        InitKeyActions(); 
    }

    /// <summary>
    /// 스킬 인풋 액션 초기화 
    /// </summary>
    private void InitKeyActions()
    {
            skillInputList.Clear(); 
    }

    /// <summary>
    /// 스킬 인풋 액션 설정 
    /// </summary>
    /// <param name="keyCodeType"></param>
    /// <param name="callback"></param>
    public void SetKeyAction(KeyCode keyCodeType,Action callback)
    {
        skillInputList.Add(new InputBinding(keyCodeType,callback));
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
    /// 모든 입력 차단 ( 죽었을 때 )
    /// </summary>
    /// <param name="isBlock"></param>
    public void BlockAllInput(bool isBlock)
    {
        _isInput = ! isBlock;
    }

    /// <summary>
    /// 플레이어 입력 차단( UI 입력 외 ) 
    /// </summary>
    /// <param name="isBlock"></param>
    public void BlockPlayerInput(bool isBlock)
    {
        _isPlayerInput = !isBlock; 
    }

    public void BlockAttackInput(bool isBlock)
    {
        _isAttackInput =!isBlock; 
    }

    /// <summary>
    /// 공격중이면 입력 차단 
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
            _moveDir = Define.MainCam.transform.TransformDirection(_moveDir);

            //OnMoveAnimation?.Invoke(MoveDir);

            if (_isMove == true)
            {
                // 이동 
                //OnMovementKeyPress?.Invoke(MoveDir);

                if (_isRotate == true) { }
                // 회전
               // CheckRotate();
            }
        }

        if(_isAttackInput == true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("클릭");
                OnDefaultAttackPress?.Invoke();
            }

            // 태클
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                OnShift?.Invoke();
            }

            // 스킬 
            for(int i =0;i < skillInputList.Count; i++)
            {
                if (Input.GetKeyDown(skillInputList[i].keyCode))
                {
                    skillInputList[i].callback?.Invoke(); 
                }
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
            RotateRay();
            //OnPointerRotate?.Invoke(Define.WorldMousePos);
        }
        else // 전투 상태면 
        {
            RotateRay(); 
        }
    }

    /// <summary>
    /// 레이쏴서 그쪽으로 회전( 레이와 충돌했을때 안했을때 따로 계산) 
    /// </summary>
    private void RotateRay()
    {
        if (Physics.Raycast(Define.MainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            OnPointerRotate?.Invoke(hitInfo.point);
        }
        else // 하늘을 찍었으면 
        {
            Vector3 originCameraPosition = Define.MainCam.transform.position;
            Vector3 dir = Define.MainCam.ScreenPointToRay(Input.mousePosition).direction;
            Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);

            Vector3 clickingPosition = ray.origin + ray.direction / ray.direction.y * (0 - originCameraPosition.y);
            OnPointerRotate?.Invoke(clickingPosition);
        }
    }
    

}
