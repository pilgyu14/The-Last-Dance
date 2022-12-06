using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    /// <summary> 
    /// <para/> �ڽĵ��� ��ȸ�ϴ� ���
    /// </summary>
    public abstract class CompositeNode : ICompositeNode
    {
        public List<INode> ChildList { get; protected set; }

        // ������
        public CompositeNode(params INode[] nodes) => ChildList = new List<INode>(nodes);

        // �ڽ� ��� �߰�
        public CompositeNode Add(INode node)
        {
            ChildList.Add(node);
            return this;
        }

        public abstract bool Run();
    }
}