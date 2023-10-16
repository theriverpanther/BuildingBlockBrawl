using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string charName;
    public float health;
    public float damage;
    public float attackRange;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        damage = 20;
        attackRange = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
