using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    //Cooldown for the aggro ability
    [SerializeField] private float aggroCooldown;
    [SerializeField] private bool aggroOnCooldown;
    //The amount of time an enemy is aggroed for
    [SerializeField] private float aggroTimer;

    [SerializeField] private int tankTargetIndex;

    // Start is called before the first frame update
    protected override void Awake()
    {
        maxHealth = 150;
        damage = 15;
        attackRange = 3;
        attackRate = 2;
        charName = "Tank";

        aggroCooldown = 10;
        aggroOnCooldown = false;
        aggroTimer = 7;

        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //for (int i = 0; i < allies.Length; i++)
        //{
        //    Unit tankUnit = allies[i].GetComponent<Unit>();

        //    Unit thisUnit = gameObject.GetComponent<Unit>();
        //    if (tankUnit == thisUnit)
        //    {
        //        tankTargetIndex = i;
        //    }
        //}

        //If the ability is on cooldown, starts the timer
        if (aggroOnCooldown)
        {
            aggroCooldown -= Time.deltaTime;
        }

        //If the timer reaches a certain amount, the ability can be used again
        if (aggroCooldown <= 0)
        {
            aggroOnCooldown = false;
            aggroCooldown = 10;
        }

        //Resets the enemy target's target index when the aggro timer is up
        if (aggroTimer <= 0)
        {
            foreach (Unit enemy in enemies)
            {
                enemy.TargetIndex = SelectTargetIndex();

                Debug.Log(enemy.name + " is no longer aggroed");
            }
            aggroTimer = 7;
        }

        //Starts the timer after using the ability
        if (aggroOnCooldown == true)
        {
            aggroTimer -= Time.deltaTime;
        }
    }

    protected override void Behaviors()
    {
        base.Behaviors();

        Aggro();
    }

    /// <summary>
    /// Will select a target based on having the most amount of damage
    /// </summary>
    /// <returns>Index of the target</returns>
    protected override int SelectTargetIndex()
    {
        if (enemies.Count == 0) return -1;
        Unit bestUnit = null;
        int mostDamage = int.MinValue;
        foreach (Unit e in enemies)
        {
            if (e.Damage > mostDamage)
            {
                mostDamage = e.Damage;
                bestUnit = e;
            }
        }
        return enemies.IndexOf(bestUnit);
    }

    /// <summary>
    /// Makes all enemy units target this unit
    /// </summary>
    private void Aggro()
    {
        if (aggroOnCooldown == false)
        {
            Debug.Log(gameObject.name + " is drawing aggro. ");

            foreach(Unit enemy in enemies)
            {
                //THIS WILL NEED TO BE UPDATED IF THE TANK IS IN A DIFFERENT POSITION
                enemy.TargetIndex = 2;
            }

            aggroOnCooldown = true;
        }
    }
}
