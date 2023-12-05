using System.Collections;
using System.Collections.Generic;
//using System.Runtime.CompilerServices;
using UnityEngine;

public class Supporter : Unit
{


    //Cooldown for the healing ability
    [SerializeField] private float healCooldown;
    [SerializeField] private bool healOnCooldown;

    //Cooldown for the debuffing ability
    [SerializeField] private float debuffCooldown;
    [SerializeField] private bool debuffOnCooldown;
    //The amount of time an enemy is debuffed for
    [SerializeField] private float debuffTimer;

    [SerializeField] private int healValue;

    // Start is called before the first frame update
    protected override void Awake()
    {
        maxHealth = 75;
        damage = 10;
        attackRange = 20;
        attackRate = 2;
        charName = "Support";

        healCooldown = 4;
        healOnCooldown = false;

        debuffCooldown = 7;
        debuffOnCooldown = false;
        debuffTimer = 5;

        healValue = 50;

        base.Awake();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //Healing cooldown
        //CheckSkillCooldown(healOnCooldown, healCooldown, 4);

        //If the ability is on cooldown, starts the timer
        if (healOnCooldown)
        {
            healCooldown -= Time.deltaTime;
        }

        //If the timer reaches a certain amount, the ability can be used again
        if (healCooldown <= 0)
        {
            healOnCooldown = false;
            healCooldown = 4;
        }

        //Debuff cooldown
        //CheckSkillCooldown(debuffOnCooldown, debuffCooldown, 7);

        if (debuffOnCooldown)
        {
            debuffCooldown -= Time.deltaTime;
        }

        //If the timer reaches a certain amount, the ability can be used again
        if (debuffCooldown <= 0)
        {
            debuffOnCooldown = false;
            debuffCooldown = 7;
        }

        //Starts the debuff timer after the debuff ability is used
        if (debuffOnCooldown == true)
        {
            debuffTimer -= Time.deltaTime;
        }
        //Gets rid of the debuff on the target once the timer is up
        if(debuffTimer <= 0)
        {
            foreach(Unit unit in enemies)
            {
                if(unit.IsDebuffed)
                {
                    unit.IsDebuffed = false;

                    Debug.Log(unit.name + " is no longer weakened");
                }
            }

            debuffTimer = 5;
        }
    }
    protected override void Behaviors()
    {
        base.Behaviors();

        // If there are enemies too close, try and back away
        if(GetClosestEnemy() <= attackRange / 3)
        {
            agent.SetDestination(transform.position + -5 * transform.forward);
            MovementSpeedChange(transform.position + -5 * transform.forward);
        }

        if (Health <= MaxHealth / 5) Heal(this);
        else HealAllies();

        DebuffEnemies();
    }

    /// <summary>
    /// Will select a target based on having the most amount of health (Tanks > DPS > Supports)
    /// </summary>
    /// <returns>Index of the target</returns>
    protected override int SelectTargetIndex()
    {
        if (enemies.Count == 0) return -1;
        Unit bestUnit = null;
        int mostHealth = int.MinValue;
        foreach (Unit e in enemies)
        {
            if(e.Health > mostHealth)
            {
                mostHealth = e.Health;
                bestUnit = e;
            }
        }
        return enemies.IndexOf(bestUnit);
    }

    private void CheckSkillCooldown(bool skillOnCooldown, float skillCooldown, float skillMaxCooldownTime)
    {
        //If the ability is on cooldown, starts the timer
        if (skillOnCooldown)
        {
            skillCooldown -= Time.deltaTime;
        }

        //If the timer reaches a certain amount, the ability can be used again
        if (skillCooldown <= 0)
        {
            skillOnCooldown = false;
            skillCooldown = skillMaxCooldownTime;
        }
    }

    /// <summary>
    /// Restore's the target's health by a certain amount
    /// </summary>
    /// <param name="target"></param>
    private void Heal(Unit target)
    {
        //If the healing ability is not on cooldown
        if(healOnCooldown == false)
        {
            //Heals the target
            target.Health += healValue;
            healOnCooldown = true;

            Debug.Log(gameObject.name + " healed " + target.name);
        }
    }

    /// <summary>
    /// Restores every allies health by a calculated amount
    /// </summary>
    private void HealAllies()
    {
        //For each ally unit
        foreach (GameObject ally in allies)
        {
            Unit unit = ally.GetComponent<Unit>();

            //If their health is below a certain threshold
            if (unit.Health <= unit.MaxHealth / 3)
            {
                //Heal them
                Heal(unit);
            }
        }
    }

    /// <summary>
    /// Lowers the target's damage output
    /// </summary>
    /// <param name="target"></param>
    private void Debuff(Unit target)
    {
        if(debuffOnCooldown == false)
        {
            target.IsDebuffed = true; 

            Debug.Log(target.name + " is weakened: ");

            debuffOnCooldown = true;
        }

    }

    /// <summary>
    /// Lowers every enemy's damage output
    /// </summary>
    private void DebuffEnemies()
    {
        //Currently just debuffs the first enemy target in the list
        foreach (Unit enemy in enemies)
        {
            Debuff(enemy);
        }
    }


}
