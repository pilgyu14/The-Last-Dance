using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    /// <summary> 조건 검사 노드 </summary>
    public class NotConditionNode : IDecoratorNode
    {
        public Func<bool> Condition { get; protected set; } // 조건 함수 
        public NotConditionNode(Func<bool> condition)
        {
            Condition = condition;
        }

        public bool Run() => !Condition();

        // Func <=> NotConditionNode 타입 캐스팅
        public static implicit operator NotConditionNode(Func<bool> condition) => new NotConditionNode(condition);
        public static implicit operator Func<bool>(NotConditionNode condition) => new Func<bool>(condition.Condition);
    }
}