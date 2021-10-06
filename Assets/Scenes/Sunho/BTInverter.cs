using System.Collections;
using System.Collections.Generic;

namespace BT
{
    public class BTInverter : BTNode
    {
        private BTNode node;

        public BTNode Node { get => node; }

        public BTInverter(BTNode node)
        {
            this.node = node;
        }

        public override NodeState Evaluate()
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    nodeState = NodeState.SUCCESS;
                    return nodeState;

                case NodeState.SUCCESS:
                    nodeState = NodeState.FAILURE;
                    return nodeState;

                case NodeState.RUNNING:
                    nodeState = NodeState.RUNNING;
                    return nodeState;
            }

            nodeState = NodeState.SUCCESS;
            return nodeState;
        }
    }
}
