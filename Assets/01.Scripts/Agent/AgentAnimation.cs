using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 애니메이션
 * 몬스터, 플레이어 
 * 
 */
[RequireComponent(typeof(Animator))]
public class AgentAnimation : MonoBehaviour
{
    protected Animator _agentAnimator;
    //Hash
    protected readonly int _walkHash = Animator.StringToHash("Walk");
    protected readonly int _deathHash = Animator.StringToHash("Death");

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

    public void PlayDeathAnimation()
    {
        _agentAnimator.SetTrigger(_deathHash);
    }

}