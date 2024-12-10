using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;

    // Activates an arrow
    public void ActivateProjectile() 
    {
        lifetime = 0;
        gameObject.SetActive(true);
    }

    // Monitors the status of the arrow
    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    // Checks if an arrow collides with a player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponent<PlayerBlock>().IsBlockingDamage(transform.position , damage);
        gameObject.SetActive(false);
    }
}
