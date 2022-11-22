using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveModule : MonoBehaviour
{
    //  캐싱 변수 
    private InputModule _inputModule;
    private CharacterController _chController;
    private MovementInfo _movementInfo;

    private float _curSpeed; 
    private Vector3 _targetPos;
    private Quaternion _targetRot; 
    // 이동 회전 

    public void Init(InputModule inputModule, CharacterController chCtrl , MovementInfo moveInfo)
    {
        this._movementInfo = moveInfo;
        this._inputModule = inputModule;
        this._chController = chCtrl; 

        // 입력 등록 
        // _inputModule.OnPointerRotate = RotateByMouse;

        // _inputModule.OnMovementKeyPress = Move;
        // _inputModule.OnMovementKeyPress = InBattleMove;
    }

    private void Update()
    {
    }


    private void RotateByMouse(Vector3 pos)
    {
        Ray ray = Define.MainCam.ScreenPointToRay(pos);
        Physics.Raycast(ray, out RaycastHit hitInfo);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 3f);

        _targetPos = hitInfo.point - transform.position;
        _targetPos.y = 0;

        _targetRot = Quaternion.LookRotation(_targetPos, Vector3.up);
        //_targetRot.x = 0;
        //_targetRot.z = 0;
    }

    /// <summary>
    /// 가속 감속 체크 
    /// </summary>
    private void CheckInput(Vector3 moveDir)
    {
        // 가속 감속 체크 
        if (moveDir.sqrMagnitude > 0) // 입력(이동) 중이면
        {
            _curSpeed += _movementInfo.acceleration * Time.deltaTime;
        }
        else
        {
            _curSpeed -= _movementInfo.deacceleration * Time.deltaTime;
        }
        _curSpeed = Mathf.Clamp(_curSpeed, 0, _movementInfo.maxSpeed);
    }

    /// <summary>
    /// 기본 상태 이동 
    /// </summary>
    /// <param name="moveDir"></param>
    public void Move(Vector3 moveDir)
    {
        CheckInput(moveDir); 

        _chController.Move(moveDir * _curSpeed);
        if (_chController.velocity.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up),
            Time.deltaTime * _curSpeed);

            _targetPos = moveDir;
        }

        // _playerAnimation.AnimatePlayer(_chController.velocity.magnitude);
    }

    /// <summary>
    /// 전투 상태 이동 
    /// </summary>
    /// <param name="moveDir"></param>
    public void InBattleMove(Vector3 moveDir)
    {
        CheckInput(moveDir); 

        Vector3 targetDir = Vector3.Normalize(moveDir.x * transform.right + moveDir.z * transform.forward); // 회전 값에 따른 이동 방향

        _chController.Move(moveDir * _curSpeed);
        //if (_chController.velocity.magnitude > 0.2f)
        //{
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot,
        Time.deltaTime * _curSpeed);
        //}

        // _playerAnimation.SetVelocity(targetDir.x, targetDir.z);

        // CheckBattle();
    }
}
