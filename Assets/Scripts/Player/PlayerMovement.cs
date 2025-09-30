using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public InputAction playerControls;
    private GameObject feet;
    private GameObject trail;
    private Rigidbody2D rigid;
    private Vector2 moveDirection = new Vector2();
    public float xForce;
    public float yForce;
    public LayerMask layers;
    public float jumpMaxTime = 0.25f;
    public float jumpCurrentTime = 0f;
    public int jumpCharges = 2;
    private bool dashAvailable = false;
    private CameraScript cameraScript;
    void Start()
    {
        feet = transform.GetChild(0).gameObject;
        trail = transform.GetChild(1).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        rigid.freezeRotation = true;
        cameraScript = Camera.main.GetComponent<CameraScript>();
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
        //Should only check once otherwise jumping inconsistent
        //Additionally should not check while jumping as will give n + 1 jumps
        if(jumpCurrentTime <= 0 && jumpCharges < 2 && Physics2D.BoxCast(feet.transform.position, new Vector2(GetComponent<BoxCollider2D>().bounds.size.x, 0.1f), 0f, new Vector2(0, -1), 0.1f, layers))
        {
            cameraScript.setMode(CameraScript.Actions.ZOOMIN);
            jumpCharges = 2;
            StartCoroutine(dashCooldown());
        }
        //#Negate gravity when starting jump
        if(Keyboard.current.upArrowKey.wasPressedThisFrame && jumpCharges > 0)
        {
            jumpCurrentTime = jumpMaxTime;
            jumpCharges -= 1;
            cameraScript.setMode(CameraScript.Actions.ZOOMOUT);
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, 0);
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
            rigid.AddForce(new Vector2(transform.right.x * 10, 0), ForceMode2D.Impulse);
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
            rigid.AddForce(new Vector2(-transform.right.x * 10, 10), ForceMode2D.Impulse);
        }
    }
    //Use fixed update for physics that are not impulses (as well as any timers that uses it)!
    void FixedUpdate()
    {
        //Only consume jump time when up arrow pressed and in the process of jumping
        if(jumpCurrentTime > 0)
        {
            if(Keyboard.current.upArrowKey.isPressed)
            {
                yForce = 1;
                jumpCurrentTime -= Time.deltaTime;
            }
            //If you cancel a jump while it's active then your jump is done (or touch the ground)
            else if (!Keyboard.current.upArrowKey.isPressed || Physics2D.BoxCast(feet.transform.position, new Vector2(GetComponent<BoxCollider2D>().bounds.size.x, 0.1f), 0f, new Vector2(0, -1), 0.1f, layers))
            {
                jumpCurrentTime = 0;
            }
        }
        rigid.AddForce(new Vector2(25 * xForce, 125 * yForce));
    }
    public IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        dashAvailable = true;
    }
}
