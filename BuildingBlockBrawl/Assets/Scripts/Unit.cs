using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    public string charName;

    public int maxHealth;
    public int currentHealth;

    public int damage;
    public float attackRange;
    public float attackRate;

    public float movementSpeed;

    public NavMeshAgent agent;

    public HealthBar healthBar;

    //[SerializeField] private DecisionMaker decisionMaker;
    //[SerializeField] private AIAction[] actions;

    //private AIAction currentAction;

    //public IEnumerator PerformAction()
    //{
    //    yield return new WaitForSeconds(1f);
    //    currentAction = decisionMaker.DecideAction(actions);

        
    //}


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
        damage = 20;
        attackRange = 3;
        attackRate = 2;
        movementSpeed = 3.5f;

        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Attacks the target unit
    /// </summary>
    public void Attack(List<GameObject> targets)
    {
        foreach(GameObject unit in targets)
        {
            Unit target = unit.GetComponent<Unit>();

            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                if (target.currentHealth > 0)
                {
                    attackRate -= Time.deltaTime;

                    if (attackRate <= 0)
                    {
                        target.currentHealth -= damage;

                        attackRate = 2;
                    }
                }
            }
        }
    }

    public virtual void Death()
    {
        if(healthBar.healthBarSprite.fillAmount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
