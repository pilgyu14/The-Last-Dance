using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    /// <summary> �ڽĵ� �� false�� ���� ������ �������� ��ȸ�ϴ� ��� </summary>
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(params INode[] nodes) : base(nodes) { }

        public override bool Run()
        {
            foreach (var node in ChildList)
            {
                bool result = node.Run();
                if (result == false)
                    return false;
            }
            return true;
        }
    }
}