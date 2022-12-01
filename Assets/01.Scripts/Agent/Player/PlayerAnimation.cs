using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AgentAnimation
{
    // Hash

    // 상태
    private readonly int _battleHash = Animator.StringToHash("Battle"); 

    // 이동 
    private readonly int _velocityXHash = Animator.StringToHash("VelocityX");
    private readonly int _velocityYHash = Animator.StringToHash("VelocityZ");

    // 공격 
    private readonly int _frontKickHash = Animator.StringToHash("FrontKick");
    private readonly int _sideKickHash = Animator.StringToHash("SideKick");
    private readonly int _backKickHash = Animator.StringToHash("BackKick");
    private readonly int _tackleHash = Animator.StringToHash("Tackle");

    // Hash 끝
    protected override void Awake()
    {
        base.Awake(); 
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

    // 이동 설정 
    public void SetVelocity(Vector3 v)
    {
        _agentAnimator.SetFloat(_velocityXHash, v.x);
        _agentAnimator.SetFloat(_velocityYHash, v.z);
    }

}
