using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class BTAction : BTNode
    {
        public delegate NodeState ActionNodeDelegate();

        private ActionNodeDelegate action;

        public BTAction(ActionNodeDelegate action)
        {
            this.action = action;
        }

        public override NodeState Evaluate()
        {
            switch (action())
            {
                case NodeState.FAILURE:
                    nodeState = NodeState.FAILURE;
                    return nodeState;

                case NodeState.SUCCESS:
                    nodeState = NodeState.SUCCESS;
                    return nodeState;

                case NodeState.RUNNING:
                    nodeState = NodeState.RUNNING;
                    return nodeState;

                default:
                    nodeState = NodeState.FAILURE;
                    return nodeState;
            }
        }
    }

}