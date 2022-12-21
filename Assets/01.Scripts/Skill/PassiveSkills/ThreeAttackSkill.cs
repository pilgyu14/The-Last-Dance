using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeAttackSkill : SkillData<PlayerController>
{
    public void Init(PlayerController playerController)
    {
        _owner = playerController; 
    }
    public override void Enter()
    {
        Debug.Log("THREE ATTACK SKILL");
        _owner.SetThreeAttackPossible(true);
    }

    public override void Stay()
    {
    }
    public override void Exit()
    {
        _owner.SetThreeAttackPossible(false);
    }

}
