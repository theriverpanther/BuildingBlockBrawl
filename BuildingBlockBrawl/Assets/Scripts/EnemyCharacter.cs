using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
        damage = 10;
        attackRange = 3;
        attackRate = 3;
        movementSpeed = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        Death();
    }
}
