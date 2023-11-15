using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class TaskAttack : Node
{
    private GameObject target;

    private float attackRate = 1f;
    private float attackCounter = 0f;

    private int attackDamage;

    public TaskAttack(GameObject target, int attackDamage)
    {
        this.target = target;
        this.attackDamage = attackDamage;
    }

    public override NodeState Evaluate()
    {
        Unit enemyTarget = target.GetComponent<Unit>();

        //increases attack counter
        attackCounter += Time.deltaTime;

        //if the counter is greater than the attack rate amount, attacks the enemy
        if(attackCounter >= attackRate)
        {
            //deals damage to the enemy target
            enemyTarget.TakeDamage(attackDamage);

            //checks for death
            enemyTarget.CheckDeath();

            //resets the counter
            attackCounter = 0f;
        }

        //continous running state
        state = NodeState.RUNNING;
        return state;
    }
}
