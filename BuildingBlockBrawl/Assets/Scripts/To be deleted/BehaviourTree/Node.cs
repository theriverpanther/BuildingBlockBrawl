using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //REFERENCE: "Create an AI with behaviour trees [Unity/C# tutorial]" by Mina Pecheux
    //https://www.youtube.com/watch?v=aR6wt5BlE-E&t=407s
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }
         
        public Node(List<Node> children)
        {
            foreach(Node child in children)
            {
                _Attach(child);
            }
        }

        /// <summary>
        /// Links child node to parent node
        /// </summary>
        /// <param name="node"></param>
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        /// <summary>
        /// Recursively finds the key and its value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key)
        {
            object value = null;

            if(_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;

            while(node != null)
            {
                value = node.GetData(key);

                if(value != null)
                {
                    return value;
                }

                node = node.parent;
            }

            return null;
        }

        /// <summary>
        /// Recursively finds the key. If found, remove it
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;

            while (node != null)
            {
                bool cleared = node.ClearData(key);

                //If root is reached, ignore request
                if (cleared)
                {
                    return true;
                }

                node = node.parent;
            }

            return false;
        }
    }
}


