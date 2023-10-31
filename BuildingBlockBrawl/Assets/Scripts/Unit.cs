using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour
{
    #region Variables
    [Header("Initialization Values")]
    public string charName;
    [SerializeField] Material playerMaterial;
    [SerializeField] Material enemyMaterial;

    [Header("Unit Statistics")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;

    [SerializeField] protected int damage;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackRate;
    [SerializeField] protected float maxAttackRate = 2f;
    [SerializeField] protected float movementSpeed;

    protected NavMeshAgent agent;
    public HealthBar healthBar;

    // public for testing, serializing would cause errors due to object removal
    public List<Unit> enemies = new List<Unit>();
    [SerializeField] protected int targetIndex = 0;
    #endregion

    #region Properties
    public int TargetIndex
    { 
        get { return targetIndex; } 
        set { 
                if (value >= 0 && value < enemies.Count)
                {
                    // If the target is changed to a proper value, recalculate the agent path
                    targetIndex = value;
                    agent.SetDestination(enemies[targetIndex].transform.position);
                    MovementSpeedChange(enemies[targetIndex].transform.position);
                }
            }
    }

    public int Health { get { return currentHealth; } }
    public int Damage { get { return damage; } }
    public float AttackRange { get { return attackRange; } }
    public float AttackRate { get { return attackRate; } }
    #endregion

    //[SerializeField] private DecisionMaker decisionMaker;
    //[SerializeField] private AIAction[] actions;

    //private AIAction currentAction;

    //public IEnumerator PerformAction()
    //{
    //    yield return new WaitForSeconds(1f);
    //    currentAction = decisionMaker.DecideAction(actions);


    //}


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Get the collection of objects on the opposing teams and add them to a list of the enemies
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag == "PlayerCharacter" ? "Enemy" : "PlayerCharacter");
        foreach (GameObject obj in objs)
        {
            enemies.Add(obj.GetComponent<Unit>());
        }
        currentHealth = maxHealth;
        movementSpeed = 3.5f;

        // Get reference to the healthbar and set the values, color, and name above it
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        healthBar.SetBasicInfo(tag, charName);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        targetIndex = SelectTargetIndex();

        // Set the material of the primitive to the corresponding color
        gameObject.GetComponent<MeshRenderer>().material = tag == "PlayerCharacter" ? playerMaterial : enemyMaterial;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Flag value to see if the agent needs a new target
        bool resetTarget = false;
        for(int i = 0; i < enemies.Count; i++)
        {
            // Loop through all enemies
            // If an enemy is inactive, remove it from the tracked list
            if (!enemies[i].isActiveAndEnabled)
            {
                enemies.RemoveAt(i);
                // If the removed enemy was the target, reset the target
                if(targetIndex == i)
                {
                    resetTarget = true;
                }
                i--;
            }
        }
        if (resetTarget) targetIndex = SelectTargetIndex();
        // Run the behavior tree
        Behaviors();
        // Move to the target
        MoveToTarget();
        // Attack whoever is in range if possible
        Attack(enemies);
        // Update the healthbar
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        // Check if the agent is dead
        CheckDeath();
    }

    /// <summary>
    /// Behavior tree of the units, overriden in each child class
    /// </summary>
    protected virtual void Behaviors()
    {
        // If the target index is within the list
        if(targetIndex > 0 && targetIndex < enemies.Count)
        {
            Unit target = enemies[targetIndex].GetComponent<Unit>();
            // Get a new target if the current target is dead
            if (target.Health <= 0 || !target.gameObject.activeSelf)
            {
                enemies.RemoveAt(targetIndex);
                targetIndex = SelectTargetIndex();
            }
        }
        // If the target isn't in range but not the designated error value, select a new target
        else if(targetIndex != -1)
        {
            targetIndex = SelectTargetIndex();
        }
    }

    /// <summary>
    /// Helper method
    /// If the target index is valid, sets agent destination and speed to the target's position
    /// </summary>
    protected void MoveToTarget()
    {
        if(targetIndex >= 0 && targetIndex < enemies.Count)
        {
            agent.SetDestination(enemies[targetIndex].transform.position);
            MovementSpeedChange(enemies[targetIndex].transform.position);
        }
    }

    /// <summary>
    /// Uses unit based logic to derive which unit to attack
    /// Overriden with different logic in each child class type
    /// </summary>
    /// <returns>The index that holds the ideal target</returns>
    protected virtual int SelectTargetIndex()
    {
        if (enemies.Count == 0) return -1;
        return Random.Range(0, enemies.Count);
    }

    /// <summary>
    /// Checks for possible attacks
    /// If there are units in range, it chooses between those units in range
    /// Will ideally choose its target, but will choose randomly if it isn't in range
    /// </summary>
    public virtual void Attack(List<Unit> targets)
    {
        List<Unit> toAttack = new List<Unit>();
        foreach(Unit unit in targets)
        {
            if (Vector3.Distance(transform.position, unit.transform.position) < attackRange)
            {
                if (unit.Health > 0)
                {
                    toAttack.Add(unit);
                }
            }
        }

        if (toAttack.Count > 0)
        {
            if(attackRate > 0)
            {
                attackRate -= Time.deltaTime;
            }
            else
            {
                if (toAttack.Contains(enemies[targetIndex]))
                {
                    enemies[targetIndex].TakeDamage(damage);
                }
                else
                {
                    int randIndex = Random.Range(0, toAttack.Count);
                    toAttack[randIndex].TakeDamage(damage);
                }
                attackRate = maxAttackRate;
            }
        }
        else
        {
            attackRate = maxAttackRate;
        }
    }

    /// <summary>
    /// Deals damage if the value is positive
    /// </summary>
    /// <param name="damageVal">Amount of damage the unit is taking</param>
    public void TakeDamage(int damageVal)
    {
        if(damageVal > 0)
        {
            currentHealth -= damage;
        }
    }

    /// <summary>
    /// If the unit's health is below 0, set it inactive
    /// </summary>
    public virtual void CheckDeath()
    {
        if(healthBar.healthBarSprite.fillAmount <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Adjusts the movement speed based on proximity to target
    /// </summary>
    /// <param name="targetPos"></param>
    public void MovementSpeedChange(Vector3 targetPos)
    {
        agent.speed = movementSpeed;
        float dist = Vector3.Distance(gameObject.transform.position, targetPos);
        //if (Vector3.Distance(gameObject.transform.position, targetPos) >= 1.0f)
        //{
        //    agent.speed = movementSpeed;
        //}
        //Stops movement upcoming coming within a certain distance of the target
        if (dist <= attackRange - 0.5f)
        {
            agent.speed = movementSpeed / Vector3.Distance(gameObject.transform.position, targetPos);
        }
        else if (dist <= attackRange)
        {
            agent.speed = 0;
        }
        
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //if (agent != null) Gizmos.DrawSphere(agent.pathEndPosition, 1f);

    }

    //public void SetTarget(int index)
    //{
    //    if (index < enemies.Count && index > 0)
    //    {
    //        targetIndex = index;
    //    }
    //}
}
