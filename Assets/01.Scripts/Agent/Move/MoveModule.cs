using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveModule : MonoBehaviour
{
    //  ĳ�� ���� 
    private InputModule _inputModule;
    private CharacterController _chController;
    private MovementInfo _movementInfo;

    private float _curSpeed; 
    private Vector3 _targetPos;
    private Quaternion _targetRot; 
    // �̵� ȸ�� 

    public void Init(InputModule inputModule, CharacterController chCtrl , MovementInfo moveInfo)
    {
        this._movementInfo = moveInfo;
        this._inputModule = inputModule;
        this._chController = chCtrl; 

        // �Է� ��� 
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
    /// ���� ���� üũ 
    /// </summary>
    private void CheckInput(Vector3 moveDir)
    {
        // ���� ���� üũ 
        if (moveDir.sqrMagnitude > 0) // �Է�(�̵�) ���̸�
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
    /// �⺻ ���� �̵� 
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
    /// ���� ���� �̵� 
    /// </summary>
    /// <param name="moveDir"></param>
    public void InBattleMove(Vector3 moveDir)
    {
        CheckInput(moveDir); 

        Vector3 targetDir = Vector3.Normalize(moveDir.x * transform.right + moveDir.z * transform.forward); // ȸ�� ���� ���� �̵� ����

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
