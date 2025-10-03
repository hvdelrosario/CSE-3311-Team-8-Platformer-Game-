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
                    text.SetText(texts[textIndex]);
                }
            }
            else
            {
                if(textIndex < texts.Count)
                {
                    text.SetText(texts[textIndex]);
                }
                else
                {
                    anim.Play("DialogueDisappear");
                    StartCoroutine(hideDialogueBox());
                }
            }
            textIndex += 1;
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
    }
}
