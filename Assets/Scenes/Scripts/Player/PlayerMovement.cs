using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallJumpPowerX;
    [SerializeField] private float wallJumpPowerY;
    [SerializeField] private float wallSlideSpeed;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
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
        
        //Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            //Makes the player move left and right
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if (onWall() && !isGrounded())
            {
                body.gravityScale = wallSlideSpeed;
                body.velocity = Vector2.zero;
            }
            else body.gravityScale = 1; 

            //Makes the player jump
            if (Input.GetKey(KeyCode.Space))
                jump();
        }
        else wallJumpCooldown += Time.deltaTime;
    }

    private void jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else if (onWall() && !isGrounded())
        {
            //if (horizontalInput == 0) {
            //    body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
            //    transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x)*5, transform.localScale.y, transform.localScale.z);
            //}else
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpPowerX, wallJumpPowerY);
            wallJumpCooldown = 0;
            
        }
        
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new UnityEngine.Vector2(0, -1), 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new UnityEngine.Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}

