using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated;
    private SpriteRenderer sprite;
    public int checkPointID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activated = false;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.yellow;
        }
    }
    
    public void activateCheckpoint()
    {
        activated = true;
    }


}
