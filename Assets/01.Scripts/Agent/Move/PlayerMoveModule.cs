using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MoveType
{
    Default, 
    Battle
}

public class PlayerMoveModule : AgentMoveModule
{
    private PlayerController owner; 
    //  캐싱 변수 
    private InputModule _inputModule;

    private PlayerAnimation _playerAnimation;
    private PlayerNMAComponent _nmaComponent;
    public override Vector3 MoveDir { get => _inputModule.MoveDir;  }

    public Action<Vector3> OnMovementAction = null; // 현재 어떤 형태로 이동하고 있는ㅈ; 

    // 상태 변수
    [SerializeField]
    private bool _isMove = true;
    [SerializeField]
    private bool _isRotate = true; 

    public override void Init(params object[] prms)
    {
        this.owner = prms[0] as PlayerController;
        this._agent = prms[1] as NavMeshAgent;
        this._movementInfo = prms[2] as MovementInfo;
        this._playerAnimation = prms[3] as PlayerAnimation;
        this._inputModule = prms[4] as InputModule;

        // 입력 등록 
        if(_inputModule != null)
        {
            _inputModule.OnPointerRotate = RotateByPos;
            _inputModule.OnKeyboardRotate = RotateDefault;
            //_inputModule.OnShift += Tackle;
        }

        _nmaComponent = new PlayerNMAComponent();
        _nmaComponent.Init(_agent);

        SetMoveMode(MoveType.Default);
    }

    protected override void Update()
    {
        base.Update();

        if(_isMove == true)
        {
            OnMovementAction?.Invoke(MoveDir);
        }
        if (_isRotate == true)
        {
            CheckRotate(); 
        }
    }

    #region 이동 회전 제한 함수 
    /// <summary>
    /// 공격중이면 입력 차단 
    /// </summary>
    /// <param name="isAttacking"></param>
    public void BlockMove(bool isBlock)
    {
        _isMove = !isBlock; 
    }

    public void BlockRotate(bool isBlock)
    {
        _isRotate = !_isRotate; 
    }
    #endregion

    #region 이동
    //@@@ 이동 @@@ //
    /// <summary>
    /// 이동 모드 설정 
    /// </summary>
    /// <param name="moveType"></param>
    public void SetMoveMode(MoveType moveType)
    {
        OnMovementAction = (moveType == MoveType.Default) ? Move : InBattleMove;
    }

    public override void Move(Vector3 moveDir)
    {
        base.Move(moveDir);
        _playerAnimation.AnimatePlayer(moveDir.sqrMagnitude);
       // Debug.Log(moveDir.sqrMagnitude);
    }

    /// <summary>
    /// 전투 상태 이동 
    /// </summary>
    /// <param name="moveDir"></param>
    public void InBattleMove(Vector3 moveDir)
    {
        CheckInput(moveDir); 

        Vector3 targetDir = Vector3.Normalize((moveDir.x * transform.right + moveDir.z * transform.forward)); // 회전 값에 따른 이동 방향

        //_chController.Move(moveDir * _curSpeed * Time.deltaTime);
        _targetDir = moveDir * _curSpeed; 
        _playerAnimation.SetVelocity(targetDir); 
 
        //if (_chController.velocity.magnitude > 0.2f)
        //{
        // transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot,
        // Time.deltaTime * _curSpeed);
        //}

        // _playerAnimation.SetVelocity(targetDir.x, targetDir.z);

        // Check
    /// <summary>Battle();
    }
    //@@@ 이동 끝 @@@ //


    //@ 태클 @ //

    public void Tackle()
    {
        // 쿨타임 체크 

        // 애니메이션 진행 상태 체크 
        if(_playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Tackle") && 
            _playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            return; 
        }

        _playerAnimation.SetTackle();
        _playerAnimation.SetVelocity(Vector2.zero);
        _playerAnimation.Update_Zero(); 
        // _inputModule.BlockPlayerInput(true); // 입력 차단 

        // 땅 쓸리는 파티클
        StartCoroutine(DashCorutine(transform.forward.normalized, 20f, 0.3f)); 
    }
    #endregion

    #region 회전
    //@@@ 회전 @@@ //

    /// <summary>
    /// 기본 상태, 전투 상태 이동 
    /// </summary>
    private void CheckRotate()
    {
        if (owner.CurState.GetType() == typeof(DefaultState))
        {
            if (MoveDir.sqrMagnitude > 0) // 키보드 입력 중이면 
            {
                RotateDefault(MoveDir);
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
            RotateByPos(hitInfo.point);
            //OnPointerRotate?.Invoke(hitInfo.point);
        }
        else // 하늘을 찍었으면 
        {
            Vector3 originCameraPosition = Define.MainCam.transform.position;
            Vector3 dir = Define.MainCam.ScreenPointToRay(Input.mousePosition).direction;
            Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);

            Vector3 clickingPosition = ray.origin + ray.direction / ray.direction.y * (0 - originCameraPosition.y);
            RotateByPos(clickingPosition); 
            //OnPointerRotate?.Invoke(clickingPosition);
        }
    }

    //@@@ 회전 끝@@@ //
    #endregion

    #region NavMeshAgent 설정
    public void SetNav10()
    {
        _nmaComponent.SetNavMeshAgent(-0.71f, 1.3f);
    }

    public void SetNav39()
    {
        _nmaComponent.SetNavMeshAgent(0, 2f);
    }
    #endregion
}
