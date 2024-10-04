using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;

    [Header ("Iframes")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;

    [Header ("Components")]
    [SerializeField] private Behaviour[] components;

    [Header ("Sounds")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    //References
    private float currentHealth;
    
    private Animator animator;
    private PlayerMovement playerMovement;
    private bool dead;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        dead = false;
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        
        if (animator != null)
        {
            if (currentHealth > 0)
            {
                animator.SetTrigger("hurt");
                SoundManager.instance.PlaySound(hurtSound);
                StartCoroutine(Invulnerability());
            } 
            else 
            { 
                if (!dead)
                {
                    animator.SetTrigger("die");
                    SoundManager.instance.PlaySound(deathSound);

                    foreach (Behaviour component in components)
                    {
                        component.enabled = false;
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

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(11,12,true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
    Physics2D.IgnoreLayerCollision(11,12,false);
    }
}