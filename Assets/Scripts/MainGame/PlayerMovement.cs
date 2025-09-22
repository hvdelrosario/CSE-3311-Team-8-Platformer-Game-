using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public InputAction playerControls;
    public GameObject feet;
    private Rigidbody2D rigid;
    private Vector2 moveDirection = new Vector2();
    private float xForce;
    private float yForce;
    public LayerMask layers;
    public bool jumpActive;
    public int jumpFrames;
    void Start()
    {
        feet = transform.GetChild(0).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        rigid.freezeRotation = true;
    }
    void OnEnable()
    {
        playerControls.Enable();
    }
    void OnDisable()
    {
        playerControls.Disable();
    }
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
        xForce = moveDirection.x;
        yForce = 0;
        //If touching ground, jump is available
        if(jumpFrames <= 0 && Physics2D.Raycast(feet.transform.position, -feet.transform.up, 0.01f, layers))
        {
           jumpFrames = 15;
        }
        //#Negate gravity when starting jump
        if(Keyboard.current.upArrowKey.wasPressedThisFrame && jumpFrames > 0)
        {
            jumpActive = true;
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, 0);
        }
        //Only consume frames when up arrow pressed and in the process of jumping
        if(jumpFrames > 0 && jumpActive)
        {
            if(Keyboard.current.upArrowKey.isPressed)
            {
                yForce = moveDirection.y;
                jumpFrames -= 1;
            }
            //If you cancel a jump while it's active then your jump is done
            else
            {
                jumpFrames = 0;
            }
        }
        if(jumpFrames <= 0)
        {
            jumpActive = false;
        }
        Debug.DrawRay(feet.transform.position, -feet.transform.up * 0.01f, Color.red, 1);
        rigid.AddForce(new Vector2(3 * xForce, 30 * yForce));
    }
}
