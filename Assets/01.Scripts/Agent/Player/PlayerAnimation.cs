using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AgentAnimation
{
    // Hash

    // 상태
    public readonly int _battleHash = Animator.StringToHash("Battle");

    // 이동 
    public readonly int _velocityXHash = Animator.StringToHash("VelocityX");
    public readonly int _velocityYHash = Animator.StringToHash("VelocityZ");

    // 공격 
  
    // 기본 공격 
    private readonly int _frontKickHash = Animator.StringToHash("FrontKick");
    private readonly int _sideKickHash = Animator.StringToHash("SideKick");
    private readonly int _backKickHash = Animator.StringToHash("BackKick");

    private readonly int _tackleHash = Animator.StringToHash("Tackle");
    private readonly int _hurricaneKickHash = Animator.StringToHash("HurricaneKick"); 

    // Hash 끝

    private List<string> _defaultAttackStrList = new List<string>(); // 기본 공격 애니메이션 해쉬 리스트  
    protected override void Awake()
    {
        base.Awake(); 
    }

    private void Start()
    {
        SetHashList(); 
    }

    /// <summary>
    /// 애니메아션 해쉬리스트에 넣기 
    /// </summary>
    private void SetHashList()
    {
        _defaultAttackStrList.Add("FrontKick");
        _defaultAttackStrList.Add("SideKick");
        _defaultAttackStrList.Add("BackKick");
    }

    /// <summary>
    /// 기본 공격 애니메이션인지 체크
    /// </summary>
    public bool CheckDefaultAnim()
    {
        foreach(var h in _defaultAttackStrList)
        {
            bool b = _agentAnimator.GetCurrentAnimatorStateInfo(0).IsName(h);
            if (b == true)
            {
                Debug.Log("A####################3");
                return true;
            }
        }
        return false; 
    }

    // 상태 설정 
    public void SetBattle(bool active)
    {
        _agentAnimator.SetBool(_battleHash, active); 
    }

    // 킥 애니메이션 (AttackSO에서 string으로 받고 리플렉션으로 실행 ) 
    public void SetFrontKick()
    {
        _agentAnimator.SetTrigger(_frontKickHash);
    }
    public void SetSideKick()
    {
        _agentAnimator.SetTrigger(_sideKickHash); 
    }
    public void SetBackKick()
    {
        _agentAnimator.SetTrigger(_backKickHash);
    }

    public void SetTackle()
    {
        _agentAnimator.SetTrigger(_tackleHash);
    }

    public void PlayHurricaneKick(bool isActive)
    {
        _agentAnimator.SetBool(_hurricaneKickHash, isActive);
    }

    // 이동 설정 
    public void SetVelocity(Vector3 v)
    {
        _agentAnimator.SetFloat(_velocityXHash, v.x);
        _agentAnimator.SetFloat(_velocityYHash, v.z);
    }

}
