using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public InputAction playerControls;
    private GameObject feet;
    private Rigidbody2D rigid;
    private Vector2 moveDirection = new Vector2();
    public float xForce;
    public float yForce;
    public LayerMask layers;
    private bool jumpActive = false;
    public float jumpTime;
    private bool dashAvailable = false;
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
        //If touching ground, jump and dash is available
        if(jumpTime <= 0 && Physics2D.Raycast(feet.transform.position, -feet.transform.up, 0.1f, layers))
        {
           jumpTime = 0.25f;
           StartCoroutine(dashCooldown());
        }
        //#Negate gravity when starting jump
        if(Keyboard.current.upArrowKey.wasPressedThisFrame && jumpTime > 0)
        {
            jumpActive = true;
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, 0);
        }
        if(jumpTime <= 0)
        {
            jumpActive = false;
        }

        if(xForce > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(xForce < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        Debug.DrawRay(feet.transform.position, -feet.transform.up * 0.01f, Color.red, 1);
        if(Keyboard.current.xKey.wasPressedThisFrame && dashAvailable)
        {
            rigid.AddForce(new Vector2(transform.right.x * 7, 0), ForceMode2D.Impulse);
            dashAvailable = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //This is how we detect the zombie head instead of the entire zomzbie
        Collider2D targetArea = collision.GetContact(0).collider;
        // Debug.Log(targetArea.gameObject.name);
        if(targetArea.gameObject.CompareTag("EnemyHitbox"))
        {
            rigid.AddForce(new Vector2(-transform.right.x * 7, 7), ForceMode2D.Impulse);
        }
    }
    //Use fixed update for physics that are not impulses! (As well as any calculations that the physics rely on)
    void FixedUpdate()
    {
        //Only consume jump time when up arrow pressed and in the process of jumping
        if(jumpTime > 0 && jumpActive)
        {
            if(Keyboard.current.upArrowKey.isPressed)
            {
                //The longer the button pressed, the less effect the force will be
                yForce = moveDirection.y * (jumpTime / 0.25f);
                jumpTime -= Time.fixedDeltaTime;
            }
            //If you cancel a jump while it's active then your jump is done
            else
            {
                jumpTime = 0;
            }
        }
        rigid.AddForce(new Vector2(25 * xForce, 100 * yForce));
    }
    public IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        dashAvailable = true;
    }
}
