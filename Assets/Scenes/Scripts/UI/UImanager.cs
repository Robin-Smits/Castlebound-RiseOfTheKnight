using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Import this namespace

public class UImanager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private PlayerInputActions playerInputActions; // Reference to the input actions

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);

        playerInputActions = new PlayerInputActions(); // Initialize the PlayerInputActions
        playerInputActions.Enable(); // Enable the input actions

        // Subscribe to the pause action
        playerInputActions.Base.Pause.performed += ctx => TogglePause();
    }

    private void OnDestroy()
    {
        playerInputActions.Disable(); // Disable the input actions when this object is destroyed
    }

    private void TogglePause()
    {
        PauseGame(pauseScreen.activeInHierarchy);
    }

    #region Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit(); // Quits the game (BUILD ONLY)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Ends playmode in editor
#endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
}
