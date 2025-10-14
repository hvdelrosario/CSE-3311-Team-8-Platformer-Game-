using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public GameObject signpost;
    public GameObject dialogueBox;
    public GameObject spring;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateSignpost(new Vector2(-12, 0.58f), new string[] {"Hello.", "You came from out of nowhere... Let's get you moving.", "Use your left and right arrow keys to move around."});
        generateSignpost(new Vector2(-4, 0.58f), new string[] {"A gap. Should be no problem however.", "Use the up arrow key in order to jump over the gap."});
        generateSignpost(new Vector2(27, 4.58f), new string[] {"Alright. This one's a bit bigger than before.", "You can boost in any direction with the x key. Try it out on this."});
        Instantiate(spring, new Vector2(61.5f, 16.58f), Quaternion.Euler(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
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
