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
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag == "PlayerCharacter" ? "Enemy" : "PlayerCharacter");
        foreach (GameObject obj in objs)
        {
            enemies.Add(obj.GetComponent<Unit>());
        }
        currentHealth = maxHealth;
        movementSpeed = 3.5f;

        healthBar = transform.GetChild(0).GetComponent<HealthBar>();
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        healthBar.SetBasicInfo(tag, charName);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        targetIndex = SelectTargetIndex();

        gameObject.GetComponent<MeshRenderer>().material = tag == "PlayerCharacter" ? playerMaterial : enemyMaterial;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        bool resetTarget = false;
        for(int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].isActiveAndEnabled)
            {
                enemies.RemoveAt(i);
                if(targetIndex == i)
                {
                    resetTarget = true;
                }
                i--;
            }
        }
        if (resetTarget) targetIndex = SelectTargetIndex();
        Behaviors();
        MoveToTarget();
        Attack(enemies);

        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        CheckDeath();
    }

    protected virtual void Behaviors()
    {
        if(targetIndex > 0 && targetIndex < enemies.Count)
        {
            Unit target = enemies[targetIndex].GetComponent<Unit>();
            //Gets a new target if the current target is dead
            if (target.Health <= 0 || !target.gameObject.activeSelf)
            {
                enemies.RemoveAt(targetIndex);
                targetIndex = SelectTargetIndex();
            }
        }
        else if(targetIndex != -1)
        {
            targetIndex = SelectTargetIndex();
        }
    }

    protected void MoveToTarget()
    {
        if(targetIndex >= 0 && targetIndex < enemies.Count)
        {
            agent.SetDestination(enemies[targetIndex].transform.position);
            MovementSpeedChange(enemies[targetIndex].transform.position);
        }
    }

    protected virtual int SelectTargetIndex()
    {
        if (enemies.Count == 0) return -1;
        return Random.Range(0, enemies.Count);
    }

    /// <summary>
    /// Attacks the target unit
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

    public void TakeDamage(int damageVal)
    {
        if(damageVal > 0)
        {
            currentHealth -= damage;
        }
    }

    public virtual void CheckDeath()
    {
        if(healthBar.healthBarSprite.fillAmount <= 0)
        {
            gameObject.SetActive(false);
        }
    }

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
