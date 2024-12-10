using UnityEngine;

public class Enemy_Vertical : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private bool movingUp;
    private float lowerEdge;
    private float upperEdge;

    void Start()
    {
        upperEdge = transform.position.y + movementDistance;
        lowerEdge = transform.position.y - movementDistance;
    }

    // Makes the item move between given positions
    private void Update()
    {
        // Move up
        if (movingUp)
        {
            if (transform.position.y < upperEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                movingUp = false;
            }
        }
        // Move down
        else
        {
            if (transform.position.y > lowerEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                movingUp = true;
            }
        }
    }

    // Checks for collision with the player and damages the given amount
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
