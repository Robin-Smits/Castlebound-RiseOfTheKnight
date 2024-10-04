using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header ("Attack settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;

    [Header ("Collider Settings")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask enemyLayer;

    [Header ("Sounds")]
    [SerializeField] private AudioClip attackSound;

    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private Health enemyHealth;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            Attack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
        SoundManager.instance.PlaySound(attackSound);
        cooldownTimer = 0;
    }

    private void Damage()
    {
        if (EnemyInSight())
        {
           enemyHealth.TakeDamage(damage); 
        }
    }

        private bool EnemyInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(
                boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(
                boxCollider.bounds.size.x * range,
                boxCollider.bounds.size.y,
                boxCollider.bounds.size.z
            ),
            0,
            Vector2.left,
            0,
            enemyLayer
            );

        if (hit.collider != null)
            enemyHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
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