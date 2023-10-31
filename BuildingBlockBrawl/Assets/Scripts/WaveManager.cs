using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    //Singleton for managers
    private static WaveManager instance;
    public static WaveManager Instance { get { return instance; } }
    //Type of Opponents and the position they will be spawned
    public List<GameObject> opponents;
    public List<Transform> opponentTransforms;
    private List<GameObject> activatedOpponents;

    //players avaters
    public List<GameObject> players;
    //public List<Transform> playerTransforms;
    //every three values considered as one wave

    //record the wave informations 
    public int opponentsPerWave;
    public List<int> waves;
    private int wave = 0, opponentsCount = 0;
    private bool nextWave=false;
    public TextMeshProUGUI waveIndicator;

    public bool NextWave { get { return nextWave; } }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        activatedOpponents = new List<GameObject>();
        players.Clear();
        players.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
    }

    // Update is called once per frame
    void Update()
    {
        //determine if players can move to the mext wave
        if (opponentsCount - opponentsPerWave != waves.Count - 1)
            nextWave = true;
        foreach (GameObject opponent in activatedOpponents)
        {
            if (opponent.activeInHierarchy == true)
            {
               nextWave = false;
            }
        }

        //Spawn new enemies if players clear the current wave
        if (nextWave && waves.Count-opponentsCount >= opponentsPerWave)
        {
            for(int i = 0; i < opponentsPerWave; i++)
            {
                GameObject opponent = SetOpponents(waves[opponentsCount + i], opponentTransforms[i]);
                opponent.tag = "Enemy";
                
                opponent.GetComponent<Unit>().Init();
                //activatedOpponents.Add(opponents[waves[wave + i]]);
                activatedOpponents.Add(opponent);
                foreach(GameObject player in players)
                {
                    opponent.GetComponent<Unit>().AddNewEnemy(player);
                    player.GetComponent<Unit>().AddNewEnemy(opponent);
                }
            }
            
            opponentsCount = opponentsCount + opponentsPerWave;
            wave++;
            
            //Update the text of the wave
            waveIndicator.text = "WAVE " + wave;
        }
    }

    void Setplayers(int playerID,Transform transform)
    {
        Instantiate(players[playerID], transform);
    }

    //create a new enemy at certain place
    GameObject SetOpponents(int opponentID, Transform transform)
    {
        GameObject opponent = Instantiate(opponents[opponentID], transform);
        //opponent.tag = "Enemy";

        return opponent;
    }

    //The condition used to check is there any enemy left. True means you win the game
    public bool WinOrNot()
    {
        bool isWin = true;

        foreach (GameObject opponent in activatedOpponents)
        {
            if (opponent.activeInHierarchy == true)
            {
                isWin = false;
            }
        }
        //int x = opponentsCount, y = waves.Count ;
        //Debug.Log(x+" - "+ y);
        
        if (activatedOpponents.Count != 0 && opponentsCount == waves.Count)
            return isWin;
        else
            return false;
    }
}
