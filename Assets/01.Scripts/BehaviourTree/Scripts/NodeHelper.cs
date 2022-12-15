using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.BehaviorTree
{
    // Core?먯꽌
    // using static Rito.BehaviorTree.NodeHelper;
    public static class NodeHelper
    {
        public static SelectorNode Selector(params INode[] nodes) => new SelectorNode(nodes);
        public static SequenceNode Sequence(params INode[] nodes) => new SequenceNode(nodes);
        public static ParallelNode Parallel(params INode[] nodes) => new ParallelNode(nodes);

        public static ConditionNode Condition(Func<bool> condition) => new ConditionNode(condition);
        public static NotConditionNode NotCondition(Func<bool> condition) => new NotConditionNode(condition);

        //      public static ActionNode RandomAction(Action[] actions) => new 
        public static ConditionNode If(Func<bool> condition) => new ConditionNode(condition);
        public static ActionNode Action(Action action) => new ActionNode(action);

        /// <summary>
        /// 조건부 실행함수
        /// </summary>
        /// <param name="condition">조건 함수</param>
        /// <param name="action">조건 만족시 실행 함수</param>
        /// <returns></returns>
        public static IfActionNode IfAction(Func<bool> condition, Action action)
            => new IfActionNode(condition, action);
        public static IfNotActionNode IfNotAction(Func<bool> condition, Action action)
        => new IfNotActionNode(condition, action);
        public static IfElseActionNode IfElseAction(Func<bool> condition, Action ifAction, Action ifElseAction)
            => new IfElseActionNode(condition, ifAction, ifElseAction);
    }
}