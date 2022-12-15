using UnityEngine;
public class EnemyAnimation : AgentAnimation
{
    protected readonly int _attack1Hash = Animator.StringToHash("Attack1");
    protected readonly int _attack2Hash = Animator.StringToHash("Attack2");

    public void PlayAttack1()
    {
        _agentAnimator.SetTrigger(_attack1Hash);
    }
    public void PlayAttack2()
    {
        _agentAnimator.SetTrigger(_attack2Hash);
    }
}
