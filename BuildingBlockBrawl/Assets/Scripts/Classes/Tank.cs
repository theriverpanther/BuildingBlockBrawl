using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        maxHealth = 150;
        damage = 15;
        attackRange = 3;
        attackRate = 2;
        charName = "Tank";

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
}
