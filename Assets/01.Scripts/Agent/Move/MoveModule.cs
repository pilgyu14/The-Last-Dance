using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveModule : MonoBehaviour
{
    //  ĳ�� ���� 
    private InputModule _inputModule;
    private CharacterController _chController;
    private PlayerAnimation _playerAnimation;

    private MovementInfo _movementInfo;

    [SerializeField] // �ӽ� ����ȭ 
    private float _curSpeed; // ���� �ӵ� 
    private Vector3 _rotTargetPos; // ��ǥ ȸ�� vector
    private Quaternion _targetRot;// ��ǥ ȸ�� quaternion
    private Vector3 _targetDir;

    [SerializeField]
    private float _velocityY = 0f;

    [SerializeField]
    private LayerMask _layerMask; 
    // �̵� ȸ�� 

    // üũ ���� 

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
        _chController.Move((_targetDir + Vector3.up * _velocityY) * Time.deltaTime);

    }

    private void RotateByMouse(Vector3 targetPos)
    {
        // Debug.Log("���콺 ȸ��"); 
        
        targetPos.y = 0; 
        Vector3 v = new Vector3(transform.position.x, 0, transform.position.z);
        _rotTargetPos = targetPos - v;
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
        _targetDir = moveDir * _curSpeed; 

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
            _velocityY += -9.8f;
        }
    }

    /// <summary>
    /// ���ݽ� �뽬 
    /// </summary>
    public void Dash()
    {
        if(Vector3.Dot(transform.forward, _inputModule.MoveDir) >0)
        {
            Debug.Log("�뽬!");
            StartCoroutine(DashCorutine(transform.forward.normalized,10f,0.1f));
        }
    }

    IEnumerator DashCorutine(Vector3 direction, float power, float duration)
    {
        direction.y = 0; 
        _targetDir = direction * power ; 
        yield return new WaitForSeconds(duration);
        StopMove(); 
    }
    
    /// <summary>
    ///  ������ ����
    /// </summary>
    public void StopMove()
    {
        _targetDir = Vector3.zero;
    }
}
