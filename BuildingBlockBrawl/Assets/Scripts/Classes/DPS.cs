using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 100;
        damage = 20;
        attackRange = 3;
        attackRate = 2;
        charName = "DPS";

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Behaviors()
    {
        base.Behaviors();
    }

    /// <summary>
    /// Will select a target based on having the least amount of health
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
}
