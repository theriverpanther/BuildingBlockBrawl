using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public string charName;

    public int maxHealth;
    public int currentHealth;

    public float damage;
    public float attackRange;

    public NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
        damage = 20;
        attackRange = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
