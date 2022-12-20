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
    //  ĳ�� ���� 
    private InputModule _inputModule;

    private PlayerAnimation _playerAnimation;
    private PlayerNMAComponent _nmaComponent;
    public override Vector3 MoveDir { get => _inputModule.MoveDir;  }

    public Action<Vector3> OnMovementAction = null; // ���� � ���·� �̵��ϰ� �ִ¤�; 

    // ���� ����
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

        // �Է� ��� 
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

    #region �̵� ȸ�� ���� �Լ� 
    /// <summary>
    /// �������̸� �Է� ���� 
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

    #region �̵�
    //@@@ �̵� @@@ //
    /// <summary>
    /// �̵� ��� ���� 
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
    /// ���� ���� �̵� 
    /// </summary>
    /// <param name="moveDir"></param>
    public void InBattleMove(Vector3 moveDir)
    {
        CheckInput(moveDir); 

        Vector3 targetDir = Vector3.Normalize((moveDir.x * transform.right + moveDir.z * transform.forward)); // ȸ�� ���� ���� �̵� ����

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
    //@@@ �̵� �� @@@ //


    //@ ��Ŭ @ //

    public void Tackle()
    {
        // ��Ÿ�� üũ 

        // �ִϸ��̼� ���� ���� üũ 
        if(_playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Tackle") && 
            _playerAnimation.AgentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            return; 
        }

        _playerAnimation.SetTackle();
        _playerAnimation.SetVelocity(Vector2.zero);
        _playerAnimation.Update_Zero(); 
        // _inputModule.BlockPlayerInput(true); // �Է� ���� 

        // �� ������ ��ƼŬ
        StartCoroutine(DashCorutine(transform.forward.normalized, 20f, 0.3f)); 
    }
    #endregion

    #region ȸ��
    //@@@ ȸ�� @@@ //

    /// <summary>
    /// �⺻ ����, ���� ���� �̵� 
    /// </summary>
    private void CheckRotate()
    {
        if (owner.CurState.GetType() == typeof(DefaultState))
        {
            if (MoveDir.sqrMagnitude > 0) // Ű���� �Է� ���̸� 
            {
                RotateDefault(MoveDir);
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
            RotateByPos(hitInfo.point);
            //OnPointerRotate?.Invoke(hitInfo.point);
        }
        else // �ϴ��� ������� 
        {
            Vector3 originCameraPosition = Define.MainCam.transform.position;
            Vector3 dir = Define.MainCam.ScreenPointToRay(Input.mousePosition).direction;
            Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);

            Vector3 clickingPosition = ray.origin + ray.direction / ray.direction.y * (0 - originCameraPosition.y);
            RotateByPos(clickingPosition); 
            //OnPointerRotate?.Invoke(clickingPosition);
        }
    }

    //@@@ ȸ�� ��@@@ //
    #endregion

    #region NavMeshAgent ����
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
