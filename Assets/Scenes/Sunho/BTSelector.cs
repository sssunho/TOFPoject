using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class BTSelector : BTNode
    {
        protected List<BTNode> nodes = new List<BTNode>();

        public BTSelector(List<BTNode> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            foreach(BTNode node in nodes)
            {
                switch(node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;

                    case NodeState.SUCCESS:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;

                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;
                }
            }

            nodeState = NodeState.FAILURE;
            return nodeState;

        }
    }
}
