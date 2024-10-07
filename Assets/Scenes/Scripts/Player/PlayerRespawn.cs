using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        if (collision.transform.tag == "FinishLevel")
        {
            // Play the checkpoint sound immediately
            SoundManager.instance.PlaySound(checkpointSound);
            
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");

            StartCoroutine(ExitLevelAfterDelay(5f));
        }
    }

    IEnumerator ExitLevelAfterDelay(float delay)
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(delay);

        // Check if the current level is the highest level completed
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex", 1))
        {
            // Update the player's progress
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();

            // Load the next level
            SceneManager.LoadScene(PlayerPrefs.GetInt("ReachedIndex"));
        }
        else
        {
            // Load the level selection menu
            SceneManager.LoadScene(1);
        }
    }
}
