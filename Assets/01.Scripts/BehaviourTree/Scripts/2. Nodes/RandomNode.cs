using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

namespace Rito.BehaviorTree
{
    /// <summary> 자식들 리턴에 관계 없이 모두 순회하는 노드 </summary>
    public class RandomNode : CompositeNode
    {
        public RandomNode(params INode[] nodes) : base(nodes) { }

        public override bool Run()
        {
            int check = Random.Range(0, ChildList.Count);
            ChildList[check].Run(); 
            return true;
        }
    }
}