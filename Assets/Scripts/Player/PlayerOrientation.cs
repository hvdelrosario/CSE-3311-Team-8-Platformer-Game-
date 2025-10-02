using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerOrientation : MonoBehaviour
{
    public float xDirection = 0f;
    public float yDirection = 0f;
    public float zDirection = 0f;
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
        zDirection = Mathf.Atan2(yDirection, xDirection) * 180 / Mathf.PI;
        //Change via angle axis instead of eulerangle cuz possible weird interaction
        //If no input just take the last known left/right angle
        if(yDirection == 0 && xDirection == 0)
        {
            if(transform.parent.gameObject.transform.right.x < 0)
            {
                zDirection = 180;
            }
            else if(transform.parent.gameObject.transform.right.x > 0)
            {
                zDirection = 0;
            }
        }
        transform.rotation = Quaternion.AngleAxis(zDirection, Vector3.forward);
    }
}
