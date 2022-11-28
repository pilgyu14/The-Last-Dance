using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveModule : MonoBehaviour
{
    //  ĳ�� ���� 
    private InputModule _inputModule;
    private CharacterController _chController;
    
    private MovementInfo _movementInfo;
    private PlayerAnimation _playerAnimation;

    [SerializeField]
    private float _curSpeed; 
    private Vector3 _rotTargetPos; //
    private Quaternion _targetRot;
    private Vector3 _targetDir;

    [SerializeField]
    private float _velocityY = 0f;

    [SerializeField]
    private LayerMask _layerMask; 
    // �̵� ȸ�� 


    public void Init(InputModule inputModule, CharacterController chCtrl , MovementInfo moveInfo, PlayerAnimation playerAnimation)
    {
        this._movementInfo = moveInfo;
        this._inputModule = inputModule;
        this._chController = chCtrl;
        this._playerAnimation = playerAnimation;

        // �Է� ��� 
        _inputModule.OnPointerRotate = RotateByMouse;
        _inputModule.OnKeyboardRotate = RotateDefault;

        // _inputModule.OnMovementKeyPress = Move;
        // _inputModule.OnMovementKeyPress = InBattleMove;
    }

    private void Update()
    {
        ApplyGravity();
        _chController.Move(_targetDir + Vector3.up * _velocityY);

    }

    private void RotateByMouse(Vector3 pos)
    {
        // Debug.Log("���콺 ȸ��"); 
        Ray ray = Define.MainCam.ScreenPointToRay(pos);
        Physics.Raycast(ray, out RaycastHit hitInfo);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 3f);

        _rotTargetPos = hitInfo.point - transform.position;
        _rotTargetPos.y = 0;

        _targetRot = Quaternion.LookRotation(_rotTargetPos, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, Time.deltaTime * _movementInfo.rotSpeed);
        
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
    /// �⺻ ���� �̵� ��
    /// </summary>
    /// <param name="moveDir"></param>
    public void Move(Vector3 moveDir)
    {
        //Debug.Log("����");
        CheckInput(moveDir);
        _targetDir = moveDir * _curSpeed * Time.deltaTime; 

        // _playerAnimation.AnimatePlayer(_chController.velocity.magnitude);
    }

    /// <summary>
    /// �⺻ �����϶� ȸ�� 
    /// </summary>
    /// <param name="moveDir"></param>
    public void RotateDefault(Vector3 moveDir)
    {
        if (_chController.velocity.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up),
            Time.deltaTime * _curSpeed);

            _rotTargetPos = moveDir;
        }

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
        _targetDir = moveDir * _curSpeed * Time.deltaTime; 
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

    /// <summary>
    /// �߷� ���� 
    /// </summary>
    private void ApplyGravity()
    {
        Ray ray = new Ray(transform.position - Vector3.up * 0.2f, -Vector3.down );
        float distance = _chController.height / 2; 
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 3f);
    
        // �ٴ��� ���̸� �߷� ����X 
        if(_chController.isGrounded == true || Physics.Raycast(ray, out RaycastHit hitInfo, distance,_layerMask) == true)
        {
            _velocityY = 0;
        }
        else
        {
            _velocityY += -9.8f * Time.deltaTime;
        }
    }

    /// <summary>
    /// ���ݽ� �뽬 
    /// </summary>
    public void Dash()
    {

    }
    /// <summary>
    ///  ������ ����
    /// </summary>
    public void StopMove()
    {
        _targetDir = Vector3.zero;
    }
}
