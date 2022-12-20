using UnityEngine;
using System;
using System.Collections.Generic;

public enum AttackType
{
    Null,
    Default_1 = 100,
    Default_2,
    Default_3,
 
    RushAttack = 300,
    Tackle = 1000, 
    HurricaneAttack
}


public class AttackModule : MonoBehaviour, IComponent
{
    // 캐싱 변수 
    private IAgent owner; 
    private FieldOfView _fov;
    private AgentMoveModule _moveModule;
    private AgentAnimation _agentAnimation; 
    private PlayerAnimation _playerAnimation; 
    private EnemyAnimation _enemyAnimation; 
    private TimerModule _timerModule; 
        

    [SerializeField]
    private bool isEnemy;

    [Header("연속 공격(플레이어만)")]
    [SerializeField]
    private AttackType _lastAttackType; // 마지막 연계 공격 타입 플레이어만 적용 
    [SerializeField]
    private bool _isThreeAttack = false; // 삼타 공격까지 가능한지 

    [Space(10)]
    [SerializeField]
    private List<AttackBase> attackInfoList = new List<AttackBase>();

    [SerializeField]
    private AttackBase _curAttackBase;
    [SerializeField]
    private LayerMask _hitLayerMask;

    // 프로퍼티
    public AttackBase CurAttackBase => _curAttackBase; 
    public AttackSO CurAttackSO => _curAttackBase.attackInfo.attackSO;
    public AttackType CurAttackType => _curAttackBase.attackInfo.attackType;
    public AttackType NextAttackType => _curAttackBase.attackInfo.nextAttackType;
    public AttackType LastAttackType
    {
        get => _lastAttackType;
        set => _lastAttackType = value; 
    }
        public bool IsThreeAttack
    {
        get => _isThreeAttack;
        set => _isThreeAttack = value; 
    }
   

    private void Start()
    {
        _hitLayerMask =  (isEnemy) ? 1 << LayerMask.NameToLayer("Player") : 1 << LayerMask.NameToLayer("Enemy");
        _curAttackBase = attackInfoList[0]; 
    }

    public void Init(IAgent owner, FieldOfView fov, AgentMoveModule moveModule ,AgentAnimation agentAnimation)
    {
        this.owner = owner; 
        _fov = fov;
        _moveModule = moveModule;
        _agentAnimation = agentAnimation; 
        
        InitAttackinfo(); 
    }

    public void DefaultAttack()
    {
        //if(CheckAttack() == true)
            //Debug.Log(_curAttackBase.attackInfo.attackSO.animationFuncName);
            if(_curAttackBase.Attack() == false && isEnemy == false) // 쿨타임 중이면서 플레이어면 
            {
                // 커서에 쿨타임 표시 
                CursorCoolTimeUI coolTimeText = PoolManager.Instance.Pop("CursorCoolTimeUI") as CursorCoolTimeUI;
                coolTimeText.UpdateCoolTimeText(_curAttackBase.RemainTime);    
            }
    }

    /// <summary>
    /// 실질적인 공격 수행 ( 애니메이션 중간 부분에 Event로 추가 ) 
    /// </summary>
    public void AttackJudge() 
    {
        Debug.Log("공격 판단");
        _curAttackBase.AttackJudge(); 
    }

    /// <summary>
    /// AttackType을 받고 AttackBase 리턴하기 없으면 NULL
    /// </summary>
    /// <param name="attackType"></param>
    /// <returns></returns>
    public AttackBase GetAttackInfo(AttackType attackType)
    {
        foreach(var attackInfo in attackInfoList)
        {
            if (attackInfo.attackInfo.attackType == attackType)
                return attackInfo; 
        }
        return null; 
    }

    public void SetCurAttackType(AttackType attackType)
    {
        attackInfoList.ForEach((x) =>
        {
            if (x.attackInfo.attackType == attackType)
            {
                _curAttackBase = x;
                return;
            }
        });
    }
    
    /// <summary>
    /// 마지막  연계 공격 타입 지정 
    /// </summary>
    /// <param name="attackType"></param>
    public void SetLastAttackType(AttackType attackType)
    {
        _lastAttackType = attackType; 
    }

    private void InitAttackinfo()
    {
        foreach (var info in attackInfoList)
        {
            info.Init(owner, _agentAnimation, _moveModule, _fov);
        }
    }

    public void ActiveFalseCollider()
    {
        // 콜라이더로 공격 판정한다면 
        if(_curAttackBase.attackCollider != null) 
        {
            _curAttackBase.attackCollider.ActiveCollider(false);// 꺼주기 
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (_curAttackInfo == null) _curAttackInfo = attackInfoList[0]; 
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawSphere(transform.position, CurAttackSO.attackRadius);

    //}
}
