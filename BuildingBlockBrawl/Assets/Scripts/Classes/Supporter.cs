using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supporter : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 75;
        damage = 10;
        attackRange = 20;
        attackRate = 2;
        charName = "Support";

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
}
