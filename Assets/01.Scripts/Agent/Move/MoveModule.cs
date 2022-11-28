using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveModule : MonoBehaviour
{
    //  캐싱 변수 
    private InputModule _inputModule;
    private CharacterController _chController;
    private PlayerAnimation _playerAnimation;

    private MovementInfo _movementInfo;

    [SerializeField] // 임시 직렬화 
    private float _curSpeed; // 현재 속도 
    private Vector3 _rotTargetPos; // 목표 회전 vector
    private Quaternion _targetRot;// 목표 회전 quaternion
    private Vector3 _targetDir;

    [SerializeField]
    private float _velocityY = 0f;

    [SerializeField]
    private LayerMask _layerMask; 
    // 이동 회전 

    // 체크 변수 

    public void Init(InputModule inputModule, CharacterController chCtrl , MovementInfo moveInfo, PlayerAnimation playerAnimation)
    {
        this._movementInfo = moveInfo;
        this._inputModule = inputModule;
        this._chController = chCtrl;
        this._playerAnimation = playerAnimation;

        // 입력 등록 
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
    /// 기본 상태 이동 ㅌ
    /// </summary>
    /// <param name="moveDir"></param>
    public void Move(Vector3 moveDir)
    {
        //Debug.Log("무브");
        CheckInput(moveDir);
        _targetDir = moveDir * _curSpeed; 

        // _playerAnimation.AnimatePlayer(_chController.velocity.magnitude);
    }

    /// <summary>
    /// 기본 상태일때 회전 
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

    /// <summary>
    /// 중력 적용 
    /// </summary>
    private void ApplyGravity()
    {
        Ray ray = new Ray(transform.position - Vector3.up * 0.2f, -Vector3.down );
        float distance = _chController.height / 2; 
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 3f);
    
        // 바닥이 땅이면 중력 적용X 
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
    /// 공격시 대쉬 
    /// </summary>
    public void Dash()
    {
        if(Vector3.Dot(transform.forward, _inputModule.MoveDir) >0)
        {
            Debug.Log("대쉬!");
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
    ///  움직임 멈춤
    /// </summary>
    public void StopMove()
    {
        _targetDir = Vector3.zero;
    }
}
