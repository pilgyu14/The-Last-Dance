using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    // ���� ���� ��� IfAction ���� �� true ����
    // ���� ������ ��� ElseAction ���� �� false ����
    /// <summary> ���ǿ� ���� ���� ��� </summary>
    public class IfElseActionNode : ILeafNode
    {
        public Func<bool> Condition { get; private set; }
        public Action IfAction { get; private set; }
        public Action ElseAction { get; private set; }

        public IfElseActionNode(Func<bool> condition, Action ifAction, Action elseAction)
        {
            Condition = condition;
            IfAction = ifAction;
            ElseAction = elseAction;
        }

        public IfElseActionNode(ConditionNode condition, ActionNode ifAction, ActionNode elseAction)
        {
            Condition = condition.Condition;
            IfAction = ifAction.Action;
            ElseAction = elseAction.Action;
        }

        public bool Run()
        {
            bool result = Condition();

            if (result) IfAction();
            else ElseAction();

            return result;
        }
    }
}