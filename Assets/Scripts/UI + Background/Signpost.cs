using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class Signpost : MonoBehaviour
{
    public GameObject dialoguePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            if(Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                dialoguePanel.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
        }
    }
}
