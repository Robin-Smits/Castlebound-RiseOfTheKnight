using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    [SerializeField] private float BlockCooldown;
    [SerializeField] private AudioClip blockSound;
    [SerializeField] private Health playerhealth;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private bool isBlocking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Block input and cooldown handling
        if (Input.GetMouseButtonDown(1) && cooldownTimer > BlockCooldown && playerMovement.canAttack())
        {
            StartBlocking();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopBlocking();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void StartBlocking()
    {
        isBlocking = true;
        animator.SetBool("block", true);
        cooldownTimer = 0;
    }

    private void StopBlocking()
    {
        animator.SetBool("block", false);
        isBlocking = false;
    }

    // Call this function to check if the player blocks damage based on the attack direction
    public void IsBlockingDamage(Vector3 enemyPosition, int _damage)
    {
        if (!isBlocking)
        {
            playerhealth.TakeDamage(_damage);
            return;
        }

        // Determine whether the enemy is on the left or right of the player
        bool enemyOnLeft = enemyPosition.x < transform.position.x;

        // Check which direction the player is facing (assuming scale.x is negative when facing left)
        bool playerFacingLeft = transform.localScale.x < 0;

        // Block if the player is facing the enemy's side
        if ((enemyOnLeft && playerFacingLeft) || (!enemyOnLeft && !playerFacingLeft))
        {
            SoundManager.instance.PlaySound(blockSound);
        }
        else
        {
            playerhealth.TakeDamage(_damage); // If not blocking correctly, take damage
        }
    }

}
