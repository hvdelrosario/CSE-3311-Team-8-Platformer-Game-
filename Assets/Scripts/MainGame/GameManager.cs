using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class GameManager : MonoBehaviour, IDataPersistence
{
    public GameObject signpost;
    public GameObject dialogueBox;
    public GameObject spring;
    public GameObject background;
    public GameObject heart;
    public GameObject player;
    public GameObject canvas;
    public GameObject checkpoint;
    public GameObject theSwitch;
    public GameObject gate;
    public GameObject pauseMenu;
    public GameObject timerPowerup;
    public GameObject timerText;
    private PlayerStats playerScript;
    public int currentLives;
    private List<GameObject> hearts;
    public List<GameObject> checkpoints;
    public int mostRecentCheckpoint;
    public Vector3 startPosition;
    public float timePassed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerScript = player.GetComponent<PlayerStats>();
        hearts = new List<GameObject>();
        checkpoints = new List<GameObject>();
    }
    void Start()
    {
        Time.timeScale = 1f;
        startPosition = new Vector3(-12, 3.5f, 0);
        timePassed = 0f;
        if (DataPersistenceManager.instance == null || !DataPersistenceManager.instance.IsFileLoaded())
        {
            player.transform.position = startPosition;
            playerScript.playerHealth = playerScript.maxHealth;
            mostRecentCheckpoint = -1;
        }
        else
        {
            Debug.Log("FILE DOES EXIST!!!!!!!!!!!!!");
            // The checkpoint will be loaded via LoadData
        }
        currentLives = playerScript.playerHealth;
        if(SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            loadTutorialLevelAssets();
        }
        else if(SceneManager.GetActiveScene().name == "Level1")
        {
            loadLevel1Assets();
        }


        for(int i = 0; i < playerScript.maxHealth; i++)
        {
            GameObject generatedHeart = Instantiate(heart, new Vector3(-800 + 150 * i, 400, 0), Quaternion.Euler(0, 0, 0));
            generatedHeart.transform.SetParent(canvas.transform, false);
            hearts.Add(generatedHeart);
        }

        // Update initial heart display based on current health
        for(int i = playerScript.maxHealth - 1; i >= playerScript.playerHealth; i--)
        {
            hearts[i].GetComponent<Animator>().Play("HeartLost");
        }
    }

    // IDataPersistence implementation
    public void LoadData(GameData data)
    {
        mostRecentCheckpoint = data.checkpointID;
        // If checkpoint is valid, respawn player at checkpoint
        if (mostRecentCheckpoint != -1 && mostRecentCheckpoint < checkpoints.Count)
        {
            player.transform.position = checkpoints[mostRecentCheckpoint].transform.position + new Vector3(0, 0.5f, 0);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.checkpointID = mostRecentCheckpoint;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        timerText.GetComponent<TextMeshProUGUI>().SetText("Timer: " + timePassed.ToString());
        if(playerScript.playerHealth < currentLives)
        {
            for(int i = currentLives - 1; i >= playerScript.playerHealth; i--)
            {
                hearts[i].GetComponent<Animator>().Play("HeartLost");
            }
        }
        currentLives = playerScript.playerHealth;

        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        if(pauseMenu.activeSelf)
        {
            Time.timeScale = 0f;  
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void generateSignpost(Vector2 location, string[] textToInsert)
    {
        GameObject temp = Instantiate(signpost, location, Quaternion.Euler(0, 0, 0));        
        temp.GetComponent<Signpost>().dialoguePanel = dialogueBox;
        List<string> texts = temp.GetComponent<Signpost>().texts;
        foreach(string text in textToInsert)
        {
            texts.Add(text);
        }
    }

    private void generateCheckpoint(Vector2 location)
    {
        GameObject temp = Instantiate(checkpoint, location, Quaternion.Euler(0, 0, 0));
        temp.GetComponent<Checkpoint>().checkPointID = checkpoints.Count;
        checkpoints.Add(temp);
    }

    public void updateCheckpoint(GameObject checkpoint)
    {
        if(checkpoint.GetComponent<Checkpoint>().checkPointID > mostRecentCheckpoint)
        {
            mostRecentCheckpoint = checkpoint.GetComponent<Checkpoint>().checkPointID;
        }
        if(!checkpoint.GetComponent<Checkpoint>().activated)
        {
            checkpoint.GetComponent<Checkpoint>().activateCheckpoint();
            
        }
    }

    private void generateSwitchGate(Vector2 switchLocation, Vector2 gateLocation, float zGateRotation)
    {
        GameObject tempSwitch = Instantiate(theSwitch, switchLocation, Quaternion.Euler(0, 0, 0));
        GameObject tempGate = Instantiate(gate, gateLocation, Quaternion.Euler(0, 0, zGateRotation));
        tempSwitch.GetComponent<Switch>().gate = tempGate;
    }

    private void generatetimerPowerup(Vector2 location, float timeCut)
    {
        GameObject temp = Instantiate(timerPowerup, location, Quaternion.Euler(0, 0, 0));
        temp.transform.GetComponent<TimerPowerup>().timeCut = timeCut;
    }
    public void exitToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    private void loadTutorialLevelAssets()
    {
        generateSignpost(new Vector2(-12, 0.58f), new string[] {"Hello.", "You came from out of nowhere... Let's get you moving.", "Use your left and right arrow keys to move around."});
        generateSignpost(new Vector2(-4, 0.58f), new string[] {"A gap. Should be no problem however.", "Use the up arrow key in order to jump over the gap."});
        generateSignpost(new Vector2(27, 4.58f), new string[] {"Alright. This one's a bit bigger than before.", "You can boost in any direction with the x key. Try it out on this."});
        generateSignpost(new Vector2(22, 4.58f), new string[] {"See that yellow platform over there? Run over it to activate the checkpoint and save your progress.", "When you respawn, you'll respawn at the last saved checkpoint."});
        generateSignpost(new Vector2(88.5f, 16.58f), new string[] {"Congratulations on completing the tutorial! Moving onto the next level."});

        generateCheckpoint(new Vector2(24.5f, 3.125f));
        
        generateSwitchGate(new Vector2(80.5f, 6.125f), new Vector2(87.125f, 17f), 0);
        Instantiate(spring, new Vector2(61.5f, 16.58f), Quaternion.Euler(0, 0, 0));

        Instantiate(background, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        //In world space, 100 pixels per unit currently
        Instantiate(background, new Vector3(-19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(background, new Vector3(19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
    }

    private void loadLevel1Assets()
    {

        generateSignpost(new Vector2(-10, 0.58f), new string[] {"Note that there's a timer during your run.", "You may be able to reduce it by collecting some stuff..."});
        generateSignpost(new Vector2(9, 8.58f), new string[] {"Mind the spikes. They do hurt."});

        Instantiate(spring, new Vector2(40.5f, 13.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(spring, new Vector2(80f, 7.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(spring, new Vector2(77.5f, 16.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(spring, new Vector2(83.5f, 20.58f), Quaternion.Euler(0, 0, 0));

        Instantiate(spring, new Vector2(88.5f, 26.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(spring, new Vector2(84.5f, 31.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(spring, new Vector2(116.5f, 11.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(spring, new Vector2(105.5f, 38.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(spring, new Vector2(85.5f, 44.58f), Quaternion.Euler(0, 0, 0));

        generateCheckpoint(new Vector2(26f, 13.125f));
        generateCheckpoint(new Vector2(82.5f, 20.125f));
        generateCheckpoint(new Vector2(101.5f, 52.125f));

        generateSwitchGate(new Vector2(62.5f, 20.125f), new Vector2(80f, 14f), 90f);


        generatetimerPowerup(new Vector2(30.5f, 23.5f), 10f);
        generatetimerPowerup(new Vector2(66f, 26.5f), 25f);
        generatetimerPowerup(new Vector2(80f, 44.5f), 15f);
        generatetimerPowerup(new Vector2(110f, 19.5f), 25f);


        Instantiate(background, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(background, new Vector3(-19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(background, new Vector3(19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
    }
}
