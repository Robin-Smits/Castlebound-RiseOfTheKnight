using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;

    [Header("Collider Settings")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip attackSound;

    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private Health enemyHealth;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInputActions = new PlayerInputActions(); // Nieuwe input acties aanmaken
    }

    private void OnEnable()
    {
        playerInputActions.Enable();

        // Abonneer je op de input acties
        playerInputActions.Base.Attack.performed += OnAttack; // Zorg dat je deze actie toevoegt in Unity
    }

    private void OnDisable()
    {
        playerInputActions.Disable();

        // Unsubscribe to prevent memory leaks
        playerInputActions.Base.Attack.performed -= OnAttack;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime; // Cooldown timer bijhouden
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (cooldownTimer > attackCooldown && playerMovement.canAttackOrBlock())
        {
            Attack();
        }
    }

    private void Attack()
    {
        playerMovement.isAttacking = true;
        animator.SetTrigger("attack");

        if (SoundManager.instance != null && attackSound != null)
        {
            SoundManager.instance.PlaySound(attackSound);
        }

        cooldownTimer = 0;
        Damage();

        StartCoroutine(ResetAttackStateAfterDelay());
    }

    private IEnumerator ResetAttackStateAfterDelay()
{
    yield return new WaitForSeconds(1f);  // Stel de tijd in gebaseerd op hoe lang de animatie duurt
    playerMovement.isAttacking = false;
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
