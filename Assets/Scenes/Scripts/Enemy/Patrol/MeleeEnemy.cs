using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask GroundLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Sounds")]
    [SerializeField] private AudioClip attackSound;

    //References
    private Animator animator;
    private Health playerHealth;
    private PlayerBlock playerBlock;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        animator.SetBool("grounded", isGrounded());

        // Attack only when the player is in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerHealth.GetCurrentHealth() > 0)
            {
                animator.SetTrigger("attack");
                SoundManager.instance.PlaySound(attackSound);
                cooldownTimer = 0;
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(
                boxCollider.bounds.size.x * range,
                boxCollider.bounds.size.y,
                boxCollider.bounds.size.z
            ),
            0,
            Vector2.left,
            0,
            playerLayer
        );

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
            playerBlock = hit.transform.GetComponent<PlayerBlock>();
        }

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerBlock.IsBlockingDamage(transform.position, damage);
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(0, -1),
            0.1f,
            GroundLayer
        );
        return raycastHit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(
                boxCollider.bounds.size.x * range,
                boxCollider.bounds.size.y,
                boxCollider.bounds.size.z
            )
        );
    }
}
