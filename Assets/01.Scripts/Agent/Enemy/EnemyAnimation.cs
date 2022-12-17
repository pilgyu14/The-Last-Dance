using UnityEngine;
public class EnemyAnimation : AgentAnimation
{
    public AnimatorOverrideController overrideController;

    protected readonly int _attackHash = Animator.StringToHash("Attack"); 
    protected readonly int _attack1Hash = Animator.StringToHash("Attack1");
    protected readonly int _attack2Hash = Animator.StringToHash("Attack2");


    protected override void Awake()
    { 
        base.Awake();
        overrideController = new AnimatorOverrideController(_agentAnimator.runtimeAnimatorController); 
        _agentAnimator.runtimeAnimatorController = overrideController;
    }

    public void ChangeAttackAnimation(AnimationClip animClip)
    {
        overrideController["GhoulAttack_1"] = animClip; 
    }
    
    public void PlayAttack()
    {
        _agentAnimator.SetTrigger(_attackHash);
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
