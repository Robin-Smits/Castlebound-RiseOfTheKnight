using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float cooldownDelay;
    [SerializeField] private float activeTime;

    [Header ("Sounds")]
    [SerializeField] private AudioClip activateSound;

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool hasDamaged;
    private bool active;

    private Health player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Starts animation if the player is colliding with the trap
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());

            player = collision.GetComponent<Health>();
        }
    }

    // Reset player if it isnt colliding with the trap
    private void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
    }

    // Check if it can damage the player
    private void Update()
    {
        if (active && player != null && !hasDamaged)
        {
            player.TakeDamage(damage);
            hasDamaged = true;
        }
    }
    // Activates the firetrap animation
    private IEnumerator ActivateFiretrap()
    {
        // Turn the sprite red to notify the player and trigger the trap
        triggered = true;
        hasDamaged = false;
        spriteRend.color = Color.red;

        // Wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(activateSound);
        spriteRend.color = Color.white; // Turn the sprite back to its initial color
        active = true;
        anim.SetBool("activated", true);

        // Wait for active time to end
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
        spriteRend.color = Color.blue;

        // Cooldown period before trap can be triggered again
        yield return new WaitForSeconds(cooldownDelay);
        spriteRend.color = Color.white;
    }
}
