using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class Signpost : MonoBehaviour
{
    public GameObject dialoguePanel;
    private bool touchingPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.downArrowKey.wasPressedThisFrame && touchingPlayer)
        {
            dialoguePanel.SetActive(true);
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
            dialoguePanel.SetActive(false);
        }
    }
}
