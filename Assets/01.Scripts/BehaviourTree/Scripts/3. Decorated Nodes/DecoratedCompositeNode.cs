using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��¥ : 2021-01-16 PM 10:20:49
// �ۼ��� : Rito

namespace Rito.BehaviorTree
{
    /// <summary> ���ǿ� ���� Composite ���� ��� </summary>
    public abstract class DecoratedCompositeNode : CompositeNode
    {
        public Func<bool> Condition { get; protected set; }

        public CompositeNode Composite { get; protected set; }

        public DecoratedCompositeNode(Func<bool> condition, CompositeNode composite)
        {
            Condition = condition;
            Composite = composite;
        }

        public override bool Run()
        {
            if (!Condition())
                return false;

            return Composite.Run();
        }
    }
}