using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS : Unit
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        maxHealth = 100;
        damage = 20;
        attackRange = 3;
        attackRate = 2;
        charName = "DPS";

        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    protected override void Behaviors()
    {
        base.Behaviors();

        CheckInstakill();
        

    }

    /// <summary>
    /// Will select a target based on having the least amount of health (Supports > DPS > Tanks)
    /// </summary>
    /// <returns>Index of the target</returns>
    protected override int SelectTargetIndex()
    {
        if (enemies.Count == 0) return -1;
        Unit bestUnit = null;
        int leastHealth = int.MaxValue;
        foreach(Unit e in enemies)
        {
            if(e.Health < leastHealth)
            {
                bestUnit = e;
                leastHealth = e.Health;
            }
        }
        return enemies.IndexOf(bestUnit);
    }

    /// <summary>
    /// Wipes out the target's remaining health (May or may not be overpowered)
    /// </summary>
    /// <param name="target"></param>
    private void Instakill(Unit target)
    {
        Debug.Log(gameObject.name + " finished off " + target.name);

        target.Health = 0;
    }

    /// <summary>
    /// Checks through every enemy to see if they can reduce their remaining health
    /// </summary>
    private void CheckInstakill()
    {
        //If an enemy target's health is low enough, finishes off the target
        foreach (Unit enemy in enemies)
        {
            if (enemy.Health <= enemy.MaxHealth / 5)
            {
                Instakill(enemy);
            }
        }
    }
}
