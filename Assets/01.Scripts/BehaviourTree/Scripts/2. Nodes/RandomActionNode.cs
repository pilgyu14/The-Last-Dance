using System;
using UnityEngine;

namespace Rito.BehaviorTree
{
    /// <summary> 랜덤 행동 수행 노드 </summary>

    public class RandomActionNode : ILeafNode
    {
       // Action[] Actions { get; protected set;  }
        public RandomActionNode(Action action)
        {

        }
        public bool Run()
        {
            throw new NotImplementedException();
        }
    }
}
