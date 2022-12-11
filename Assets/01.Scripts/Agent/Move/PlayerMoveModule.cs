using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveModule : AgentMoveModule<PlayerController>
{
    //  캐싱 변수 
    private InputModule _inputModule;

    private PlayerAnimation _playerAnimation;
    private PlayerNMAComponent _nmaComponent;
    public override Vector3 MoveDir { get => _inputModule.MoveDir;  }


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
            _inputModule.OnPointerRotate = RotateByMouse;
            _inputModule.OnKeyboardRotate = RotateDefault;
            _inputModule.OnShift += Tackle;
        }
    }

    private void RotateByMouse(Vector3 targetPos)
    {
        // Debug.Log("마우스 회전"); 
        
        targetPos.y = 0; 
        Vector3 v = new Vector3(transform.position.x, 0, transform.position.z);
        _rotTargetPos = targetPos - v;
        _rotTargetPos.y = 0;

        _targetRot = Quaternion.LookRotation(_rotTargetPos, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, Time.deltaTime * _movementInfo.rotSpeed);
        
        //_targetRot.x = 0;
        //_targetRot.z = 0;
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


    // 태클 관련 
    public void Tackle()
    {
        _playerAnimation.SetTackle();
        // 땅 쓸리는 파티클
        StartCoroutine(DashCorutine(transform.forward.normalized, 20f, 0.3f)); 
    }

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
