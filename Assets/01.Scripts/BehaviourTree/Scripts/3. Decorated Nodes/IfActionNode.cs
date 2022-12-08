using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    // ���� ���� ��� Action ���� �� true ����
    // ���� ������ ��� false ����
    /// <summary> ���ǿ� ���� ���� ��� </summary>
    public class IfActionNode : ActionNode
    {
        public Func<bool> Condition { get; private set; }

        public IfActionNode(Func<bool> condition, Action action)
            : base(action)
        {
            Condition = condition;
        }
        public IfActionNode(ConditionNode condition, ActionNode action)
            : base(action.Action)
        {
            Condition = condition.Condition;
        }

        public override bool Run()
        {
            bool result = Condition();
            if (result) Action();
            return result;
        }
    }
}