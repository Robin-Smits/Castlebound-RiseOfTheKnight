using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private int playerLives;

    [Header("Sounds")]
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UImanager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UImanager>();
    }

    public void CheckRespawn()
    {
        if (playerLives > 0)
        {
            //Respawn player & take a life away
            playerLives--;
            transform.position = currentCheckpoint.position;
            playerHealth.Respawn();
        }
        else
        {
            //Game over
            uiManager.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
