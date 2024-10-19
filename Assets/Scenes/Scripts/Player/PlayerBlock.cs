using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    [SerializeField] private float BlockCooldown;
    [SerializeField] private AudioClip blockSound;
    [SerializeField] private Health playerhealth;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private bool isBlocking;

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
        playerInputActions.Base.Block.performed += OnBlock;
        playerInputActions.Base.Block.canceled += OnBlockRelease;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();

        // Unsubscribe to prevent memory leaks
        playerInputActions.Base.Block.performed -= OnBlock;
        playerInputActions.Base.Block.canceled -= OnBlockRelease;
    }

    private void Update()
    {
        // Cooldown bijhouden
        cooldownTimer += Time.deltaTime;
    }

    private void OnBlock(InputAction.CallbackContext context)
    {
        if (cooldownTimer > BlockCooldown && playerMovement.canAttackOrBlock())
        {
            StartBlocking();
        }
    }

    private void OnBlockRelease(InputAction.CallbackContext context)
    {
        StopBlocking();
    }

    private void StartBlocking()
    {
        isBlocking = true;
        playerMovement.isBlocking = true;
        animator.SetBool("block", true);
        playerMovement.enabled = false;
        cooldownTimer = 0; // Reset cooldown timer bij blokkeren
    }

    private void StopBlocking()
    {
        animator.SetBool("block", false);
        playerMovement.isBlocking = false;
        playerMovement.enabled = true;
        isBlocking = false;
    }

    // Call this function to check if the player blocks damage based on the attack direction
    public void IsBlockingDamage(Vector3 enemyPosition, float _damage)
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
