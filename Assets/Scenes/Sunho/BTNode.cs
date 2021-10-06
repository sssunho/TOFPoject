using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    public enum NodeState { FAILURE, SUCCESS, RUNNING }

    [System.Serializable]
    public abstract class BTNode
    {
        public delegate NodeState NodeRetun();

        protected NodeState nodeState;

        public NodeState NodeState { get => nodeState; }

        public BTNode() { }

        public abstract NodeState Evaluate();
    }
}
