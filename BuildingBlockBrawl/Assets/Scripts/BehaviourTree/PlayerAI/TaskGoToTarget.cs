using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;

public class TaskGoToTarget : Node
{
    private Transform transform;
    private GameObject target;
    private NavMeshAgent agent;

    //constructor
    public TaskGoToTarget(Transform transform, GameObject target, NavMeshAgent agent)
    {
        this.transform = transform;
        this.target = target;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        //if the target is not within a certain range (attack range)
        if(Vector3.Distance(transform.position, target.transform.position) > 1.0f)
        {
            //go to the target position
            agent.SetDestination(target.transform.position);
        }

        //returns a continous running state
        state = NodeState.RUNNING;
        return state;
    }
}
