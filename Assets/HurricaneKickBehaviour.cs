using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneKickBehaviour : StateMachineBehaviour
{
    private PlayerController _owner;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _owner ??= animator.GetComponentInParent<PlayerController>();
       
        _owner.InputModule.BlockAttackInput(true);        // 공격 차단 
        _owner.MoveModule.BlockRotate(false); // 회전 안 차단 
        _owner.AttackModule.AttackJudge(); 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _owner.MoveModule.RotatePlace(); // 제자리 회전 
        _owner.CheckEndHurricaneKick(); // 지속 시간 지나면 끝 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _owner.InputModule.BlockAttackInput(false);
        _owner.MoveModule.BlockRotate(false); 
        _owner.AttackModule.GetAttackInfo(AttackType.HurricaneAttack).attackCollider.ColliderFalse(); 
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
