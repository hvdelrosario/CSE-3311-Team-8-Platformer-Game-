using UnityEngine;

public class Sun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void LateUpdate()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 9);
    }
}
