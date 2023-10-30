using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public abstract class Unit : MonoBehaviour
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

    public NavMeshAgent agent;
    public HealthBar healthBar;

    protected List<Unit> enemies = new List<Unit>();
    protected int targetIndex = 0;
    #endregion

    #region Properties
    public int TargetIndex
    { 
        get { return targetIndex; } 
        set { 
                if (value >= 0 && value < enemies.Count)
                {
                    targetIndex = value;
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
        //Debug.Log("agent assignment");

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
                agent.SetDestination(enemies[targetIndex].transform.position);
            }

            
        }
        else if(targetIndex != -1)
        {
            targetIndex = SelectTargetIndex();
            //Moves to the current target
            agent.SetDestination(enemies[targetIndex].transform.position);
            MovementSpeedChange(enemies[targetIndex].transform.position);
            Attack(enemies);
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
    public void Attack(List<Unit> targets)
    {
        foreach(Unit unit in targets)
        {
            if (Vector3.Distance(transform.position, unit.transform.position) < attackRange)
            {
                if (unit.Health > 0)
                {
                    attackRate -= Time.deltaTime;

                    if (attackRate <= 0)
                    {
                        unit.currentHealth -= damage;

                        attackRate = maxAttackRate;
                    }
                }
            }
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
        if (Vector3.Distance(gameObject.transform.position, targetPos) >= 1.0f)
        {
            agent.speed = movementSpeed;
        }
        //Stops movement upcoming coming within a certain distance of the target
        if (Vector3.Distance(gameObject.transform.position, targetPos) <= attackRange - 0.5f)
        {
            agent.speed = 0;
        }
    }

    //public void SetTarget(int index)
    //{
    //    if (index < enemies.Count && index > 0)
    //    {
    //        targetIndex = index;
    //    }
    //}
}
