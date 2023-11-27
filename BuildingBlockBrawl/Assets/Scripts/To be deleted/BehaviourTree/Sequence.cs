using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //REFERENCE: "Create an AI with behaviour trees [Unity/C# tutorial]" by Mina Pecheux
    //https://www.youtube.com/watch?v=aR6wt5BlE-E&t=407s

    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunnning = false;

            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    //if any child fails, return failure
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunnning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunnning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}

