using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : State<PlayerController>
{

    public override void Enter()
    {
        owner.MoveModule.SetMoveMode(MoveType.Default);
        //owner.InputModule.OnMovementKeyPress += owner.MoveModule.Move;
        //owner.InputModule.OnMoveAnimation += owner.MoveDefaultAnimation;
        // move ³Ö±â 
    }
    public override void Stay()
    {

    }

    public override void Exit()
    {
        //owner.InputModule.OnMovementKeyPress -= owner.MoveModule.Move;
        //owner.InputModule.OnMoveAnimation -= owner.MoveDefaultAnimation;
    }

}