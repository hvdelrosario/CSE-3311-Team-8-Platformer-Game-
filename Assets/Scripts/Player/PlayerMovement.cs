using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public InputAction playerControls;
    private GameObject feet;
    private GameObject trail;
    private GameObject playerOrientation;
    private Rigidbody2D rigid;
    public float xForce;
    public float yForce;
    public LayerMask layers;
    public float jumpMaxTime = 0.25f;
    public float jumpCurrentTime = 0f;
    public int jumpMaxCharges = 1;
    public int jumpCharges = 0;
    public float dashMaxCooldown = 0.4f;
    public float dashCooldown;
    private bool touchingGround = false;
    private CameraScript cameraScript;
    public Animator anim;
    void Start()
    {
        feet = transform.GetChild(0).gameObject;
        trail = transform.GetChild(1).gameObject;
        playerOrientation = transform.GetChild(2).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        rigid.freezeRotation = true;
        cameraScript = Camera.main.GetComponent<CameraScript>();
        anim = GetComponent<Animator>();
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
        xForce = 0;
        yForce = 0;
        if(Keyboard.current.leftArrowKey.isPressed)
        {
            xForce -= 1;
        }
        if(Keyboard.current.rightArrowKey.isPressed)
        {
            xForce += 1;
        }
        //If touching ground, jump and dash is available
        //Should only check once otherwise jumping inconsistent
        //Additionally should not check while jumping as will give n + 1 jumps
        touchingGround = Physics2D.BoxCast(feet.transform.position, new Vector2(GetComponent<BoxCollider2D>().bounds.size.x, 0.1f), 0f, new Vector2(0, -1), 0.1f, layers);
        if(jumpCurrentTime <= 0 && jumpCharges < jumpMaxCharges && touchingGround)
        {
            cameraScript.setMode(CameraScript.Actions.ZOOMIN);
            jumpCharges = jumpMaxCharges;
        }
        //#Negate gravity when starting jump or dashing
        if(Keyboard.current.upArrowKey.wasPressedThisFrame && jumpCharges > 0)
        {
            jumpCurrentTime = jumpMaxTime;
            jumpCharges -= 1;
            cameraScript.setMode(CameraScript.Actions.ZOOMOUT);
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, 0);
        }
        if(Keyboard.current.xKey.wasPressedThisFrame && dashCooldown <= 0)
        {
            rigid.linearVelocity = new Vector3(0, 0, 0);
            //Thinking about cutting off jumpforce if dashing so that they're indepdendent, but makes for a cool combo effect if we keep both in, figure it out later
            jumpCurrentTime = 0;
            rigid.AddForce(new Vector2(playerOrientation.transform.right.x * 15, playerOrientation.transform.right.y * 25), ForceMode2D.Impulse);
            //Dashes should reset immediately upon touching the ground and only have the extended cooldown when moving across ground
            if(touchingGround)
            {
                dashCooldown = dashMaxCooldown;
            }
            else
            {
                dashCooldown = 0.01f;
            }
        }
        if(touchingGround)
        {
            dashCooldown -= Time.deltaTime;
        }
        if(xForce > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        else if(xForce < 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        Debug.DrawRay(feet.transform.position, -feet.transform.up * 0.01f, Color.red, 1);

        if(touchingGround)
        {
            if(rigid.linearVelocity.x > 0.1f || rigid.linearVelocity.x < -0.1f)
            {
                anim.Play("PlayerWalk");
            }
            else
            {
                anim.Play("New State");
            }
        }
        else
        {
            anim.Play("Airbound");
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
        if(collision.gameObject.CompareTag("Spring"))
        {
            rigid.AddForce(collision.gameObject.transform.up * 35, ForceMode2D.Impulse);
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
}
