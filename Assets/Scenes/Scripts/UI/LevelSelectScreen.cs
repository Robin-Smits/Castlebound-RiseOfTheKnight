using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip interactSound;

    [Header("Buttons")]
    [SerializeField] private Button[] buttons;
    private void Awake()
    {
        //Only make LVL 1 available on launch
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetInt("ReachedIndex", 1);
            PlayerPrefs.SetInt("UnlockedLevel", 1);
        }
    }

    public void OpenLevel(int levelID)
    {
        SceneManager.LoadScene(levelID + 1);
    }

    public void MainMenu()
    {
        SoundManager.instance.PlaySound(interactSound);
        SceneManager.LoadScene(0);
    }

}
