using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Unit
{
    //public GameObject enemy;
    //public List<GameObject> enemies;

    //private int randomTarget;

    //private int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = 100;
        //currentHealth = maxHealth;
        //damage = 20;
        //attackRange = 3;
        //attackRate = 2;
        //movementSpeed = 3.5f;

        //Creates a list of all enemies in the scene
        //enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //Finds a random target to attack (MAY BE REMOVED IN THE FUTURE IF RANDOMNESS IS NOT NECCESSARY)
        //randomTarget = Random.Range(0, enemyCount);

        //enemyCount = enemies.Count;

        //Debug.Log(randomTarget);
    }

    // Update is called once per frame
    void Update()
    {
        //Unit target = enemies[randomTarget].GetComponent<Unit>();

        ////Gets a new target if the current target is dead
        //if(target.currentHealth <= 0)
        //{
        //    randomTarget = Random.Range(0, enemyCount);
        //}

        ////Moves to the current target
        //agent.SetDestination(enemies[randomTarget].transform.position);

        //MovementSpeedChange(enemies[randomTarget].transform.position);

        //Attack(enemies);
        //healthBar.UpdateHealthBar(maxHealth, currentHealth);
        //Death();
    }

    public void AddNewEnemy(GameObject newEnemy)
    {
        enemies.Add(newEnemy);
        //Finds a random target to attack (MAY BE REMOVED IN THE FUTURE IF RANDOMNESS IS NOT NECCESSARY)
        enemyCount = enemies.Count;
        randomTarget = Random.Range(0, enemyCount);

        
    }
}
