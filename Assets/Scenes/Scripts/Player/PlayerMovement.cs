using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float cayoteTime;
    private float cayoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumps")]
    [SerializeField] private float wallJumpPowerX;
    [SerializeField] private float wallJumpPowerY;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpCooldownDuration;
    private float wallJumpCooldown;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();

        // Subscribe to input actions
        playerInputActions.Base.Move.performed += OnMove;
        playerInputActions.Base.Move.canceled += OnMove;
        playerInputActions.Base.Jump.performed += OnJump;
        playerInputActions.Base.Jump.canceled += OnJumpRelease;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();

        // Unsubscribe to prevent memory leaks
        playerInputActions.Base.Move.performed -= OnMove;
        playerInputActions.Base.Move.canceled -= OnMove;
        playerInputActions.Base.Jump.performed -= OnJump;
        playerInputActions.Base.Jump.canceled -= OnJumpRelease;
    }

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.freezeRotation = true;
    }

    private void Update()
    {
        // Switch animations
        animator.SetBool("running", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        // Wall sliding logic
        if (onWall() && !isGrounded())
        {
            // Set the velocity directly for wall sliding
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlideSpeed, 0));
        }
        else
        {
            // Reset gravity and apply horizontal movement when not on a wall
            body.gravityScale = 1;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // Handle jump conditions
            if (isGrounded())
            {
                cayoteCounter = cayoteTime;
                jumpCounter = extraJumps;
            }
            else
            {
                cayoteCounter -= Time.deltaTime;
            }
        }

        // Handle wall jump cooldown
        if (wallJumpCooldown > 0)
        {
            wallJumpCooldown -= Time.deltaTime;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontalInput = input.x;

        // Flip player based on direction
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(4, 4, 4); // Facing right
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-4, 4, 4); // Facing left
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jump();
    }

    private void OnJumpRelease(InputAction.CallbackContext context)
    {
        if (body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
    }

    private void wallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpPowerX, wallJumpPowerY));
        SoundManager.instance.PlaySound(jumpSound);
        wallJumpCooldown = wallJumpCooldownDuration;
    }

    // This method checks if the player can wall jump, enhancing the wall detection logic.
    private bool canWallJump()
    {
        return onWall() && wallJumpCooldown <= 0;
    }

    // Enhanced jump method that uses the new canWallJump() method.
    private void jump()
    {
        // Check if we can jump (coyote time and wall jump logic)
        if (cayoteCounter < 0 && !canWallJump()) return;

        // Execute wall jump if on a wall
        if (canWallJump())
        {
            wallJump();
        }
        else
        {
            // Normal jump logic
            if (isGrounded())
            {
                SoundManager.instance.PlaySound(jumpSound);
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else if (cayoteCounter > 0)
            {
                SoundManager.instance.PlaySound(jumpSound);
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                cayoteCounter = 0; // Reset coyote time
            }
            else if (jumpCounter > 0)
            {
                SoundManager.instance.PlaySound(jumpSound);
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                jumpCounter--;
            }
        }
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(0, -1), 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
private void OnDrawGizmos()
{
    // Check if boxCollider is assigned
    if (boxCollider == null)
    {
        // Try to get the BoxCollider2D component if it's not assigned
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Draw ground check gizmos
    Gizmos.color = Color.green;
    Vector2 groundCheckOrigin = (Vector2)boxCollider.bounds.center + Vector2.down * (boxCollider.bounds.extents.y + 0.1f);
    Gizmos.DrawRay(groundCheckOrigin, Vector2.down * 0.1f); // Draw line down for ground check
    Gizmos.DrawWireCube(groundCheckOrigin, new Vector2(boxCollider.bounds.size.x, 0.2f)); // Draw a wire cube to represent the ground check area

    // Draw wall check gizmos
    Gizmos.color = Color.blue;
    Vector2 wallCheckOrigin = (Vector2)boxCollider.bounds.center + new Vector2(boxCollider.bounds.extents.x * Mathf.Sign(transform.localScale.x), 0);
    Gizmos.DrawRay(wallCheckOrigin, new Vector2(transform.localScale.x, 0) * 0.1f); // Draw line to the side for wall check
    Gizmos.DrawWireCube(wallCheckOrigin, new Vector2(0.2f, boxCollider.bounds.size.y)); // Draw a wire cube to represent the wall check area
}



}
