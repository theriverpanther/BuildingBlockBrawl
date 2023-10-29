using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Unit
{
    //public GameObject enemy;
    public List<GameObject> enemies;

    private int randomTarget;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
        damage = 20;
        attackRange = 3;
        attackRate = 2;
        movementSpeed = 3.5f;

        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        randomTarget = Random.Range(0, enemies.Count);

        Debug.Log(randomTarget);
    }

    // Update is called once per frame
    void Update()
    {



        //Debug.Log("TEST");
        agent.SetDestination(enemies[randomTarget].transform.position);


        if(Vector3.Distance(gameObject.transform.position, enemies[randomTarget].transform.position) >= 1.0f)
        {
            agent.speed = movementSpeed;
        }
        //Stops movement upcoming coming within a certain distance of the target
        if (Vector3.Distance(gameObject.transform.position, enemies[randomTarget].transform.position) <= 2.5f)
        {
            agent.speed = 0;
        }

        Attack(enemies);

        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        Death();
    }
}
