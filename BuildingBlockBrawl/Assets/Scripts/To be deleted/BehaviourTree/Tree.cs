using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //REFERENCE: "Create an AI with behaviour trees [Unity/C# tutorial]" by Mina Pecheux
    //https://www.youtube.com/watch?v=aR6wt5BlE-E&t=407s

    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            //if it has a tree, evaluate it continously
            if(_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}

