using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject signpost;
    public GameObject dialogueBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject temp = Instantiate(signpost, new Vector3(9, 3, 0), Quaternion.Euler(0, 0, 0));
        // GameObject tempText = temp.GetChild(0).gameObject;
        temp.GetComponent<Signpost>().dialoguePanel = dialogueBox;
        temp = Instantiate(signpost, new Vector3(12, 3, 0), Quaternion.Euler(0, 0, 0));
        temp.GetComponent<Signpost>().dialoguePanel = dialogueBox;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
