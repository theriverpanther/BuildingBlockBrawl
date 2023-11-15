using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //REFERENCE: "Create an AI with behaviour trees [Unity/C# tutorial]" by Mina Pecheux
    //https://www.youtube.com/watch?v=aR6wt5BlE-E&t=407s

    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                        //return early if child has succeeded
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}

