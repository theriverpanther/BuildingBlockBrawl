using System.Collections;
using System.Collections.Generic;
//using System.Runtime.CompilerServices;
using UnityEngine;

public class Supporter : Unit
{
    //THIS COULD POTENTIALLY BE MOVED INTO THE UNIT PARENT SCRIPT
    //Array of teammates
    [SerializeField] private GameObject[] allies;

    //Cooldown for the healing ability
    public float healCooldown;
    public bool healOnCooldown;

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

        base.Awake();

        //If it is the player's supporter character
        if(gameObject.tag == "PlayerCharacter")
        {
            //Add all player characters into the array
            allies = GameObject.FindGameObjectsWithTag("PlayerCharacter");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //If the healing ability is on cooldown, starts the timer
        if(healOnCooldown)
        {
            healCooldown -= Time.deltaTime;
        }

        //If the timer reaches a certain amount, the healing ability can be used again
        if(healCooldown <= 0)
        {
            healOnCooldown = false;
            healCooldown = 4;
        }
    }
    protected override void Behaviors()
    {
        base.Behaviors();

        //For each ally unit
        foreach(GameObject ally in allies)
        {
            Unit unit = ally.GetComponent<Unit>();

            //If their health is below a certain threshold
            if(unit.Health <= 30)
            {
                //Heal them
                Heal(unit);
            }
        }
    }

    /// <summary>
    /// Will select a target based on having the most amount of health
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
            target.Health += 50;
            healOnCooldown = true;
        }
    }
}
