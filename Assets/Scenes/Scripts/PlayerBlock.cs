using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    [SerializeField] private float BlockCooldown;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(1) && cooldownTimer > BlockCooldown && playerMovement.canAttack())
        {
            Block();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Block()
    {
        animator.SetTrigger("block");
        cooldownTimer = 0;
    }
}