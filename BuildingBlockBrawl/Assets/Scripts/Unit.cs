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
    [SerializeField] protected Material playerMaterial;
    [SerializeField] Material enemyMaterial;
    protected Vector3 spawnPos;

    [Header("Unit Statistics")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;

    [SerializeField] protected int damage;
    [SerializeField] protected int maxDamage;

    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackRate;
    [SerializeField] protected float maxAttackRate = 2f;
    [SerializeField] protected float movementSpeed;

    protected NavMeshAgent agent;
    public HealthBar healthBar;

    // public for testing, serializing would cause errors due to object removal
    public List<Unit> enemies = new List<Unit>();

    //Array of teammates
    public GameObject[] allies;

    [SerializeField] protected int targetIndex = 0;

    protected bool isDebuffed;
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

    public int Health { get { return currentHealth; } set { currentHealth = value; } }
    public int MaxHealth { get { return maxHealth; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int MaxDamage {  get { return maxDamage; } }
    public float AttackRange { get { return attackRange; } }
    public float AttackRate { get { return attackRate; } }


    public bool IsDebuffed {  get { return isDebuffed; } set { isDebuffed = value; } }
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
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        maxDamage = damage;
        spawnPos = transform.position;

        movementSpeed = 3.5f;

        isDebuffed = false;

        // Get reference to the healthbar and set the values, color, and name above it
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();
        healthBar.UpdateHealthBar(maxHealth, currentHealth);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        targetIndex = SelectTargetIndex();

        //If it is the player's character
        if (gameObject.tag == "PlayerCharacter")
        {
            //Add all player characters into the array
            allies = GameObject.FindGameObjectsWithTag("PlayerCharacter");
        }


        // Done with helper for the sake of the wave manager
        Init();
    }

    public void Init()
    {
        enemies.Clear();
        healthBar.SetBasicInfo(tag, charName);
        // Set the material of the primitive to the corresponding color
        gameObject.GetComponent<MeshRenderer>().material = tag == "PlayerCharacter" ? playerMaterial : enemyMaterial;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //If gameobject is an enemy
        if (gameObject.tag == "Enemy")
        {
            //Constantly checks for other enemy allies due to them spawning in later
            allies = GameObject.FindGameObjectsWithTag("Enemy");
        }

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
        // Update the healthbar
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        // Check if the agent is dead
        CheckDeath();

        //If the unit is debuffed, halves their damage output
        if(isDebuffed && (damage == maxDamage))
        {
            damage = damage / 2;
        }
        //else, their damage is normal
        else
        {
            damage = maxDamage;
        }
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

        // If the unit is healthy, move towards the target, otherwise hide from the enemies
        if (currentHealth / (float)maxHealth > 0.25f) MoveToTarget();
        else Hide();

        // Attack whoever is in range if possible
        Attack(enemies);
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
    /// Flee away from combat
    /// </summary>
    protected void Hide()
    {
        // If the unit is close to some enemies, go to where they had spawned
        if (GetClosestEnemy() <= 5)
        {
            agent.SetDestination(spawnPos);
            MovementSpeedChange(spawnPos); 
        }

        // If they are far away but still need to hide (like in the case of a supporter)
        // Turn away from the enemies and go towards that direction
        else
        {
            Vector3 sharedDir = Vector3.zero;
            foreach(Unit e in enemies)
            {
                sharedDir += e.transform.forward;
            }
            sharedDir = sharedDir.normalized;
            sharedDir = new Vector3(sharedDir.y, -sharedDir.x, sharedDir.z);
            agent.SetDestination(transform.position + sharedDir * 5);
            MovementSpeedChange(transform.position + sharedDir * 5);
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

    public void AddNewEnemy(GameObject newEnemy)
    {
        enemies.Add(newEnemy.GetComponent<Unit>());
        targetIndex = SelectTargetIndex();

    }

    protected float GetClosestEnemy()
    {
        float closestEnemy = float.MaxValue;
        float distance = 0f;
        foreach (Unit enemy in enemies)
        {
            distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < closestEnemy) closestEnemy = distance;
        }
        return closestEnemy;
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
