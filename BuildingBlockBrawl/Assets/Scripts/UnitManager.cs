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
        // Get reference to both the player agents and the enemy agents at start
        playerAgents.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        enemyAgents.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // If the player clicks on a valid object
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit))
            {
                //Debug.DrawRay(r.origin, r.direction, Color.red, 2f);
                // If the valid object is a player agent, clear current selection and select the unit that was clicked
                if(hit.collider.gameObject.tag == "PlayerCharacter")
                {
                    ClearSelectedIndicator();
                    hit.collider.transform.GetChild(1).gameObject.SetActive(true);
                    selectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                }
                // If the valid object was an enemy and there was a selected unit already, change the target of the unit to the enemy clicked on
                else if(hit.collider.gameObject.tag == "Enemy" && selectedUnit != null)
                {
                    selectedUnit.TargetIndex = enemyAgents.IndexOf(hit.collider.gameObject);
                }
                // If neither of those options worked, clear the selected object(s)
                else
                {
                    RemoveSelection();
                }
            }
            // If the raycast failed, clear the selected object(s)
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
        //// Win State
        //else if(WaveManager.Instance.opponents.Count == 0 && !WaveManager.Instance.NextWave)
        //{
        //    menu.gameObject.SetActive(true);
        //    menu.transform.GetChild(0).GetComponent<Image>().color = victoryColor;
        //    menu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Victory";
        //}
    }

    /// <summary>
    /// Helper method
    /// Clears both the selected unit as well as all of the indicator game objects
    /// </summary>
    void RemoveSelection()
    {
        selectedUnit = null;
        ClearSelectedIndicator();
        //Debug.Log("Clear");
    }

    /// <summary>
    /// Helper method
    /// Loops through all of the player agents and sets their indicators to inactive
    /// </summary>
    void ClearSelectedIndicator()
    {
        foreach (GameObject agent in playerAgents)
        {
            agent.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Event called on the reset button is clicked
    /// </summary>
    public void RetryGame()
    {
        // Before selection this is a good option, if we have unit selection we instead send them to unit selection screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Event called when the main menu button is clicked
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Event called when the main menu button is clicked
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
