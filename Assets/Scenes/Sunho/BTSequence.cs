using System.Collections;
using System.Collections.Generic;

namespace BT
{
    public class BTSequence : BTNode
    {
        List<BTNode> nodes = new List<BTNode>();

        public BTSequence(List<BTNode> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;

            foreach(BTNode node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        return nodeState;

                    case NodeState.SUCCESS:
                        continue;

                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        continue;

                    default:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                }
            }

            nodeState = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;

            return nodeState;
        }

    }
}