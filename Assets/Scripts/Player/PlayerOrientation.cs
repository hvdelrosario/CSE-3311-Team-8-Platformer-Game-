using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerOrientation : MonoBehaviour
{
    private float xDirection = 0f;
    private float yDirection = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xDirection = 0;
        yDirection = 0;
        if(Keyboard.current.leftArrowKey.isPressed)
        {
            xDirection -= 1;
        }
        if(Keyboard.current.rightArrowKey.isPressed)
        {
            xDirection += 1;
        }
        if(Keyboard.current.downArrowKey.isPressed)
        {
            yDirection -= 1;
        }
        if(Keyboard.current.upArrowKey.isPressed)
        {
            yDirection += 1;
        }
        //Mathatan2 considers all quadrants
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(yDirection, xDirection));
    }
}
