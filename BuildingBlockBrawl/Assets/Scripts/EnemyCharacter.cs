using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Unit
{
    public List<GameObject> players;

    private int randomTarget;

    private int playerCount;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        damage = 10;
        attackRange = 3;
        attackRate = 3;
        movementSpeed = 3.5f;

        players.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));

        randomTarget = Random.Range(0, playerCount);

        playerCount = players.Count;
    }

    // Update is called once per frame
    void Update()
    {
        Unit target = players[randomTarget].GetComponent<Unit>();

        //Gets a new target if the current target is dead
        if (target.currentHealth <= 0)
        {
            randomTarget = Random.Range(0, playerCount);
        }

        //Moves to the current target
        agent.SetDestination(players[randomTarget].transform.position);

        MovementSpeedChange(players[randomTarget].transform.position);

        Attack(players);
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        Death();
    }
}
