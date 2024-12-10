using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    // Checks for collision with the player and damages the given amount
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}
