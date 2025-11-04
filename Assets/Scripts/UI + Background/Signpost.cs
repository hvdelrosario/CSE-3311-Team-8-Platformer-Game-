using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class Signpost : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject spaceKey;
    public TextMeshProUGUI text;
    private Animator anim;
    //Every signpost should have at least one text
    public List<string> texts;
    public int textIndex;
    public bool touchingPlayer;
    public bool skipActivated = false;
    public bool finishedDialogue = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Need to put this in awake to be created before adding stuff into the list
    void Awake()
    {
        texts = new List<string>();

    }
    void Start()
    {
        anim = dialoguePanel.GetComponent<Animator>();
        text = dialoguePanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        spaceKey = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame && touchingPlayer)
        {
            Time.timeScale = 0f;  
            if(!dialoguePanel.activeSelf)
            {
                textIndex = 0;
                dialoguePanel.SetActive(true);
                anim.Play("DialoguePopUp");
                if(texts.Count == 0)
                {
                    text.SetText("No text assigned to this signpost!");
                }
                else
                {
                    StartCoroutine(textAppear(texts[textIndex]));
                    textIndex += 1;
                }
            }
            else
            {
                if(textIndex < texts.Count)
                {
                    if(finishedDialogue)
                    {
                        StartCoroutine(textAppear(texts[textIndex]));
                        textIndex += 1;
                    }
                    else
                    {
                        skipActivated = true;
                    }
                }
                else
                {
                    //Might still be in the middle of the last text, finish it before closing
                    if(!finishedDialogue)
                    {
                        skipActivated = true;
                    }
                    else
                    {
                        anim.Play("DialogueDisappear");
                        StartCoroutine(hideDialogueBox());
                    }

                }
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            spaceKey.SetActive(true);
            touchingPlayer = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
            spaceKey.SetActive(false);
            if(dialoguePanel.activeSelf)
            {
                anim.Play("DialogueDisappear");
                //Need to delay a bit before calling disappear for the animation to play
                StartCoroutine(hideDialogueBox());
            }
        }
    }

    public IEnumerator hideDialogueBox()
    {
        yield return new WaitForSeconds(0.1f);
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;  
    }

    public IEnumerator textAppear(string selectedText)
    {
        for(int i = 0; i < selectedText.Length; i++)
        {
            finishedDialogue = false;
            if(skipActivated)
            {
                i = selectedText.Length - 1;
                skipActivated = false;
            }
            text.SetText(selectedText.Substring(0, i+1));
            yield return new WaitForSeconds(0.02f);
        }
        finishedDialogue = true;
    }
}
