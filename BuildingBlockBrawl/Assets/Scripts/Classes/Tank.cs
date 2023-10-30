using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 150;
        damage = 15;
        attackRange = 3;
        attackRate = 2;
        charName = "Tank";

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

    // Tank will currently evaluate units with the highest damage as the best targets
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
