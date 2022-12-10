using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    /// <summary> �ڽĵ� ���Ͽ� ���� ���� ��� ��ȸ�ϴ� ��� </summary>
    public class ParallelNode : CompositeNode
    {
        public ParallelNode(params INode[] nodes) : base(nodes) { }

        public override bool Run()
        {
            foreach (var node in ChildList)
            {
                node.Run();
            }
            return true;
        }
    }
}