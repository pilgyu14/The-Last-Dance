using System;
using UnityEngine; 

public interface IAgentInput
{
    public Action<Vector3> OnMovementKeyPress { get; set; } // 움직임 
    public Action<Vector3> OnPointerRotate { get; set; } // 포인터에 대한 회전 
    
    public Action OnDefaultAttackPress { get; set; } // 기본 공격 
}
