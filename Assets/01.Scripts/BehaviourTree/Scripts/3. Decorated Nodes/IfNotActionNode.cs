using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��¥ : 2021-01-13 PM 4:20:16

namespace Rito.BehaviorTree
{
    // ���� ������ ��� Action ���� �� true ����
    // ���� ���� ��� false ����
    /// <summary> ���ǿ� ���� ���� ��� </summary>
    public class IfNotActionNode : ILeafNode
    {
        public Func<bool> Condition { get; private set; }
        public Action Action { get; private set; }
        public IfNotActionNode(Func<bool> condition, Action action)
        {
            Condition = () => !condition();
            Action = action;
        }
        public IfNotActionNode(ConditionNode condition, ActionNode action)
        {
            Condition = () => !condition.Condition();
            Action = action.Action;
        }

        public bool Run()
        {
            bool result = Condition();
            if (result) Action();
            return result;
        }
    }
}