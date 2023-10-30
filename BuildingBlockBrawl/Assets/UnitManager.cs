using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<GameObject> playerAgents = new List<GameObject>();
    private List<GameObject> enemyAgents = new List<GameObject>();

    // Changing hierarchy after alpha for a better suited set of behaviors
    [SerializeField]
    private PlayerCharacter selectedUnit = null;

    // Start is called before the first frame update
    void Start()
    {
        playerAgents.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        enemyAgents.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit))
            {
                Debug.DrawRay(r.origin, r.direction, Color.red, 2f);
                if(hit.collider.gameObject.tag == "PlayerCharacter")
                {
                    ClearSelectedIndicator();
                    hit.collider.transform.GetChild(1).gameObject.SetActive(true);
                    selectedUnit = hit.collider.gameObject.GetComponent<PlayerCharacter>();
                }
                else if(hit.collider.gameObject.tag == "Enemy" && selectedUnit != null)
                {
                    selectedUnit.SetTarget(enemyAgents.IndexOf(hit.collider.gameObject));
                }
                else
                {
                    RemoveSelection();
                }
            }
            else
            {
                RemoveSelection();
            }
        }
    }

    void ClearSelectedIndicator()
    {
        foreach (GameObject agent in playerAgents)
        {
            agent.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void RemoveSelection()
    {
        selectedUnit = null;
        ClearSelectedIndicator();
        Debug.Log("Clear");
    }
}
