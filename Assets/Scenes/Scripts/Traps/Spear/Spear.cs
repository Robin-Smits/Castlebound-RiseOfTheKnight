using System.Collections;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float timeExtended;
    [SerializeField] private float cooldownDelay;

    [Header("Sounds")]
    [SerializeField] private AudioClip activateSound;

    private Animator anim;
    private bool hasDamaged;
    private bool active;

    private Health player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // Start the automatic activation cycle when the game begins
        StartCoroutine(ActivateSpearRepeatedly());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<Health>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = null;
        }
    }

    private void Update()
    {
        if (active && player != null && !hasDamaged)
        {
            player.TakeDamage(damage);
            hasDamaged = true;
        }
    }

    private IEnumerator ActivateSpearRepeatedly()
    {
        while (true)
        {
            hasDamaged = false;

            //Extend the spear
            anim.SetTrigger("extend");
            SoundManager.instance.PlaySound(activateSound);
            active = true;

            yield return new WaitForSeconds(0.5f);
            anim.SetTrigger("extended");

            yield return new WaitForSeconds(timeExtended);

            // Retract the spear
            anim.SetTrigger("retract");

            yield return new WaitForSeconds(0.5f);
            active = false;

            anim.SetTrigger("idle");

            // Cooldown period before the next cycle
            yield return new WaitForSeconds(cooldownDelay);
        }
    }
}
