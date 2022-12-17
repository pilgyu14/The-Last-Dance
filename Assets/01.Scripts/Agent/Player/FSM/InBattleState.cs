using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBattleState : State<PlayerController>
{
    private TimerModule timer;
    public override void Enter()
    {
        owner.StartBattle();
        owner.InputModule.OnMovementKeyPress += owner.MoveModule.InBattleMove;
        timer = new TimerModule(4f, () => owner.EndBattleState());
        // InBattlemove ³Ö±â 
        // StartBattle
    }
    public override void Stay()
    {
        timer.UpdateSomething(); 
       //owner.CheckBattle();
        //CheckBattle
    }

    public override void Exit()
    {
        owner.InputModule.OnMovementKeyPress -= owner.MoveModule.InBattleMove;
        timer = null;
    }
}
