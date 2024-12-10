using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement")]
    [SerializeField] private float speed;

    [Header("Idle")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator animator;
    private Vector3 initScale;
    private bool movingLeft;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        animator.SetBool("moving", false);
    }

    // Makes the enemy move between the given positions
    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                ChangeDirection();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                ChangeDirection();
            }
        }
    }

    // Makes the enemy move in the given direction
    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        animator.SetBool("moving", true);
        //Face the direction it's moving
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        //Move in a direction
        enemy.position = new Vector3(
            enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y,
            enemy.position.z
            );
    }

    // Changes which way the enemy is moving and facing
    private void ChangeDirection()
    {
        animator.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }
}
