using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AgentAnimation
{
    // Hash

    // 이동 
    private readonly int _velocityXHash = Animator.StringToHash("VelocityX");
    private readonly int _velocityYHash = Animator.StringToHash("VelocityY");

    // 공격 
    private readonly int _frontKickHash = Animator.StringToHash("FrontKick");
    private readonly int _sideKickHash = Animator.StringToHash("SideKick");
    private readonly int _backKickHash = Animator.StringToHash("BackKick");
    private readonly int _tackleHash = Animator.StringToHash("Tackle");

    protected override void Awake()
    {
        
    }

    // 킥 애니메이션 
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
    public void SetVelocity(float x, float y)
    {
        _agentAnimator.SetFloat(_velocityXHash, x);
        _agentAnimator.SetFloat(_velocityYHash, y);
    }

}
