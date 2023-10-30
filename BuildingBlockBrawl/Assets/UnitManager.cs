using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    private List<GameObject> playerAgents = new List<GameObject>();
    private List<GameObject> enemyAgents = new List<GameObject>();

    // Changing hierarchy after alpha for a better suited set of behaviors
    [SerializeField] private Unit selectedUnit = null;

    [Header("UI")]
    [SerializeField] private Color failColor;
    [SerializeField] private Color victoryColor;
    [SerializeField] private Canvas menu;

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
                //Debug.DrawRay(r.origin, r.direction, Color.red, 2f);
                if(hit.collider.gameObject.tag == "PlayerCharacter")
                {
                    ClearSelectedIndicator();
                    hit.collider.transform.GetChild(1).gameObject.SetActive(true);
                    selectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                }
                else if(hit.collider.gameObject.tag == "Enemy" && selectedUnit != null)
                {
                    Debug.Log(selectedUnit.TargetIndex);
                    selectedUnit.TargetIndex = enemyAgents.IndexOf(hit.collider.gameObject);
                    Debug.Log(selectedUnit.TargetIndex);
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

        // Clear managed list based on active game objects
        for (int i = 0; i < playerAgents.Count; i++)
        {
            if (!playerAgents[i].activeSelf)
            {
                playerAgents.Remove(playerAgents[i]);
                i--;
            }
        }
        for(int i = 0; i < enemyAgents.Count; i++)
        {
            if (!enemyAgents[i].activeSelf)
            {
                enemyAgents.Remove(enemyAgents[i]);
                i--;
            }
        }
        
        // Lose State
        if(playerAgents.Count == 0)
        {
            menu.gameObject.SetActive(true);
            menu.transform.GetChild(0).GetComponent<Image>().color = failColor;
            menu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Defeat!";
        }
        // Win State
        else if(enemyAgents.Count == 0)
        {
            menu.gameObject.SetActive(true);
            menu.transform.GetChild(0).GetComponent<Image>().color = victoryColor;
            menu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Victory";
        }
    }

    void RemoveSelection()
    {
        selectedUnit = null;
        ClearSelectedIndicator();
        //Debug.Log("Clear");
    }

    void ClearSelectedIndicator()
    {
        foreach (GameObject agent in playerAgents)
        {
            agent.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void RetryGame()
    {
        // Before selection this is a good option, if we have unit selection we instead send them to unit selection screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
