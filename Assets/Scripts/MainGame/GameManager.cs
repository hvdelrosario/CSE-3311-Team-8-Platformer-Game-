using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public GameObject signpost;
    public GameObject dialogueBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject temp = Instantiate(signpost, new Vector3(9, 3, 0), Quaternion.Euler(0, 0, 0));        
        temp.GetComponent<Signpost>().dialoguePanel = dialogueBox;
        List<string> texts = temp.GetComponent<Signpost>().texts;
        texts.Add("Hi, how are you");
        texts.Add("Yeah ok bye");
        Debug.Log("GOod");
        temp = Instantiate(signpost, new Vector3(12, 3, 0), Quaternion.Euler(0, 0, 0));
        temp.GetComponent<Signpost>().dialoguePanel = dialogueBox;
        texts = temp.GetComponent<Signpost>().texts;
        texts.Add("hfdkajfdhskjafda");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
