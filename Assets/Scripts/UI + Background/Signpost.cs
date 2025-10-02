using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class Signpost : MonoBehaviour
{
    public GameObject dialoguePanel;
    private Animator anim;
    private bool touchingPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = dialoguePanel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame && touchingPlayer)
        {
            if(!dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(true);
                anim.Play("DialoguePopUp");
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
            anim.Play("DialogueDisappear");
            StartCoroutine(hideDialogueBox());
            //Need to delay a bit before calling disappear
        }
    }

    public IEnumerator hideDialogueBox()
    {
        yield return new WaitForSeconds(0.1f);
        dialoguePanel.SetActive(false);
    }
}
