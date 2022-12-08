using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    /// <summary> �ൿ ���� ��� </summary>
    public class ActionNode : ILeafNode
    {
        public Action Action { get; protected set; }
        public ActionNode(Action action)
        {
            Action = action;
        }

        public virtual bool Run()
        {
            Action();
            return true;

        }

        // Action <=> ActionNode Ÿ�� ĳ����
        public static implicit operator ActionNode(Action action) => new ActionNode(action);
        public static implicit operator Action(ActionNode action) => new Action(action.Action);
    }
}