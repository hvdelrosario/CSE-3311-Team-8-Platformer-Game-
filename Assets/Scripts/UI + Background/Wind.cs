using UnityEngine;

public class Wind : MonoBehaviour
{
    public float windStrength;
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
            collider.gameObject.GetComponent<Rigidbody2D>().AddForce(-windStrength * transform.right);
        }
    }
}
