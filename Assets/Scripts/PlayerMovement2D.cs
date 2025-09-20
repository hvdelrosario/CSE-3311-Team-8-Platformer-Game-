using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public LayerMask groundMask;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;

    // This class is auto-generated when you checked "Generate C# Class" on your Input Actions asset
    private InputSystem_Actions controls;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new InputSystem_Actions();

        // Hook up Move action
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Hook up Jump action
        controls.Player.Jump.performed += ctx => TryJump();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // Horizontal movement (we only use X from the Vector2)
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // Simple ground check with a raycast downwards
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundMask);
    }

    void TryJump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
