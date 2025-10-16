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
    private PlayerStats playerScript;
    public int currentLives;
    private List<GameObject> hearts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerScript = player.GetComponent<PlayerStats>();
    }
    void Start()
    {
        generateSignpost(new Vector2(-12, 0.58f), new string[] {"Hello.", "You came from out of nowhere... Let's get you moving.", "Use your left and right arrow keys to move around."});
        generateSignpost(new Vector2(-4, 0.58f), new string[] {"A gap. Should be no problem however.", "Use the up arrow key in order to jump over the gap."});
        generateSignpost(new Vector2(27, 4.58f), new string[] {"Alright. This one's a bit bigger than before.", "You can boost in any direction with the x key. Try it out on this."});
        Instantiate(spring, new Vector2(61.5f, 16.58f), Quaternion.Euler(0, 0, 0));
        Instantiate(background, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        //In world space, 100 pixels per unit currently
        Instantiate(background, new Vector3(-19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(background, new Vector3(19.20f, 0, 0), Quaternion.Euler(0, 0, 0));
        currentLives = playerScript.playerHealth;
        hearts = new List<GameObject>();
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
}
