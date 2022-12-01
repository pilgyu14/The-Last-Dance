using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AgentAnimation
{
    // Hash

    // ����
    private readonly int _battleHash = Animator.StringToHash("Battle"); 

    // �̵� 
    private readonly int _velocityXHash = Animator.StringToHash("VelocityX");
    private readonly int _velocityYHash = Animator.StringToHash("VelocityZ");

    // ���� 
    private readonly int _frontKickHash = Animator.StringToHash("FrontKick");
    private readonly int _sideKickHash = Animator.StringToHash("SideKick");
    private readonly int _backKickHash = Animator.StringToHash("BackKick");
    private readonly int _tackleHash = Animator.StringToHash("Tackle");

    // Hash ��
    protected override void Awake()
    {
        base.Awake(); 
    }

    // ���� ���� 
    public void SetBattle(bool active)
    {
        _agentAnimator.SetBool(_battleHash, active); 
            
    }

    // ű �ִϸ��̼� (AttackSO���� string���� �ް� ���÷������� ���� ) 
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

    // �̵� ���� 
    public void SetVelocity(Vector3 v)
    {
        _agentAnimator.SetFloat(_velocityXHash, v.x);
        _agentAnimator.SetFloat(_velocityYHash, v.z);
    }

}
