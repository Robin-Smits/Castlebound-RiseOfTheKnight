using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
    
    private Animator animator;
    private PlayerMovement playerMovement;
    private bool dead;

    private void Awake()
    {
        dead = false;
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        
        if (animator != null)
        {
            if (currentHealth > 0)
            {
                animator.SetTrigger("hurt");
            } 
            else 
            { 
                if (!dead)
                {
                    animator.SetTrigger("die");
                    if (playerMovement != null)
                    {
                        playerMovement.enabled = false;
                    }
                    dead = true;
                }
            }
        }
    }
    public void AddHealth(float _health)
    {
        currentHealth = Mathf.Clamp(currentHealth + _health, 0, startingHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1f);
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}