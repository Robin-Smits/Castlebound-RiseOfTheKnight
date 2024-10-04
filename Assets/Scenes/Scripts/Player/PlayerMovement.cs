using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Setttings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Cayote time")]
    [SerializeField] private float cayoteTime;
    private float cayoteCounter;

    [Header("Multiple jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumps")]
    [SerializeField] private float wallJumpPowerX;
    [SerializeField] private float wallJumpPowerY;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpCooldownDuration;
    private float wallJumpCooldown;

    [Header("Layer settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(4, 4, 4);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-4, 4, 4);

        //Switch animations
        animator.SetBool("running", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        //Jump 
        if (Input.GetKeyDown(KeyCode.Space))
            jump();

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        // Wall sliding logic
        if (onWall() && !isGrounded())
        {
            // Player slides down the wall
            body.velocity = new Vector2(body.velocity.x, -wallSlideSpeed);

            // Reduce gravity to slow down the fall
            body.gravityScale = wallSlideSpeed;
        }
        else
        {
            // Reset gravity to normal and apply horizontal movement
            body.gravityScale = 1;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

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
        
        if (wallJumpCooldown > 0)
        {
            wallJumpCooldown -= Time.deltaTime;
        }
    }

    private void jump()
    {
        if (cayoteCounter < 0 && !onWall()) return;

        if (onWall() && wallJumpCooldown <= 0)
            wallJump();
        else
        {
            SoundManager.instance.PlaySound(jumpSound);
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                if (cayoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            //Reset counter to avoid dubblejumps
            cayoteCounter = 0;
        }
    }

    private void wallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpPowerX, wallJumpPowerY));
        SoundManager.instance.PlaySound(jumpSound);
        wallJumpCooldown = wallJumpCooldownDuration;
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

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}

