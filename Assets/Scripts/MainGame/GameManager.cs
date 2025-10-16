using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public GameObject signpost;
    public GameObject dialogueBox;
    public GameObject spring;
    public GameObject background;
    public GameObject heart;
    public GameObject player;
    public GameObject canvas;
    public GameObject checkpoint;
    private PlayerStats playerScript;
    public int currentLives;
    private List<GameObject> hearts;
    public List<GameObject> checkpoints;
    public int mostRecentCheckpoint;
    public Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerScript = player.GetComponent<PlayerStats>();
        hearts = new List<GameObject>();
        checkpoints = new List<GameObject>();
    }
    void Start()
    {
        startPosition = new Vector3(-12, 3.5f, 0);
        player.transform.position = startPosition;

        generateSignpost(new Vector2(-12, 0.58f), new string[] {"Hello.", "You came from out of nowhere... Let's get you moving.", "Use your left and right arrow keys to move around."});
        generateSignpost(new Vector2(-4, 0.58f), new string[] {"A gap. Should be no problem however.", "Use the up arrow key in order to jump over the gap."});
        generateSignpost(new Vector2(27, 4.58f), new string[] {"Alright. This one's a bit bigger than before.", "You can boost in any direction with the x key. Try it out on this."});
        generateSignpost(new Vector2(22, 4.58f), new string[] {"See that yellow platform over there? Run over it to activate the checkpoint and save your progress.", "When you respawn, you'll respawn at the last saved checkpoint."});
        generateSignpost(new Vector2(88.5f, 16.58f), new string[] {"Congratulations on completing the tutorial! Moving onto the next level."});

        mostRecentCheckpoint = -1;
        generateCheckpoint(new Vector2(24.5f, 3.125f));
        
        Instantiate(spring, new Vector2(61.5f, 16.58f), Quaternion.Euler(0, 0, 0));


        Instantiate(background, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        //In world space, 100 pixels per unit currently
        Instantiate(background, new Vector3(-19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(background, new Vector3(19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
        currentLives = playerScript.playerHealth;

        for(int i = 0; i < playerScript.maxHealth; i++)
        {
            GameObject generatedHeart = Instantiate(heart, new Vector3(-800 + 150 * i, 400, 0), Quaternion.Euler(0, 0, 0));
            generatedHeart.transform.SetParent(canvas.transform, false);
            hearts.Add(generatedHeart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.playerHealth < currentLives)
        {
            for(int i = currentLives - 1; i >= playerScript.playerHealth; i--)
            {
                hearts[i].GetComponent<Animator>().Play("HeartLost");
            }
        }
        currentLives = playerScript.playerHealth;
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
}
