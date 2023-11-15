using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviourTree;

public class PlayerBT : BehaviourTree.Tree
{
    [SerializeField] private int attackDamage;

    //MAY OR MAY NOT NEED TO BE CHANGED TO INT TARGETINDEX IF THAT IS HOW WE ARE DOING TARGETING
    [SerializeField] GameObject target;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //private new void Start()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //}

    protected override Node SetupTree()
    {
        //PART OF THIS WAS COPIED OVER FROM MY HOMEWORK ASSSIGNMENT
        Node root = new Selector(new List<Node>
        {
             new Sequence(new List<Node>
            {
                //attack
                new TaskAttack(target, attackDamage),
            }),
            new Sequence(new List<Node>
            {
                //detection/movement
                new TaskGoToTarget(transform, target, agent),
            }),
            //movement
            //new TaskPatrol(transform,waypoints),
        });

        return root;
    }
}
