using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 애니메이션
 * 몬스터, 플레이어 
 * 
 */
[RequireComponent(typeof(Animator))]
public class AgentAnimation : MonoBehaviour, IComponent
{
    protected Animator _agentAnimator;
    //Hash
    protected readonly int _walkHash = Animator.StringToHash("Walk");
    protected readonly int _deathHash = Animator.StringToHash("Death");
    protected readonly int _hitHash = Animator.StringToHash("Hit"); 

    public Animator AgentAnimator => _agentAnimator;
    protected virtual void Awake()
    {
        _agentAnimator = GetComponent<Animator>();
    }

    public void SetWalkAnimation(bool value)
    {
        _agentAnimator.SetBool(_walkHash, value);
    }

    public void AnimatePlayer(float velocity)
    {
        SetWalkAnimation(velocity > 0);
    }

    public void PlayHitAnimation()
    {
        _agentAnimator.SetTrigger(_hitHash); 
    }

    public void PlayDeathAnimation()
    {
        _agentAnimator.SetTrigger(_deathHash);
    }

    public void Update_Zero()
    {
        _agentAnimator.Update(0);
    }

    public bool GetCurrentAnimationState(int hashCode)
    {
        return _agentAnimator.GetCurrentAnimatorStateInfo(0).GetHashCode() == hashCode;
    
    }
}