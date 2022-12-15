using UnityEngine;
public class EnemyAnimation : AgentAnimation
{
    public AnimatorOverrideController overrideController;

    protected readonly int _attack1Hash = Animator.StringToHash("Attack1");
    protected readonly int _attack2Hash = Animator.StringToHash("Attack2");

    private void Awake()
    {
        base.Awake();
        overrideController.runtimeAnimatorController = _agentAnimator.runtimeAnimatorController;
    }

    public void ChangeAttackAnimation(AnimationClip animClip)
    {
        overrideController["Attack"] = animClip; 
    }
    public void PlayAttack1()
    {
        _agentAnimator.SetTrigger(_attack1Hash);
    }
    public void PlayAttack2()
    {
        _agentAnimator.SetTrigger(_attack2Hash);
    }
}
