using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip interactSound;

    //Loads level select scene
    public void StartGame()
    {
        SoundManager.instance.PlaySound(interactSound);
        SceneManager.LoadScene(1);
    }

    public void SoundVolume()
    {
        SoundManager.instance.PlaySound(interactSound);
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.PlaySound(interactSound);
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

    public void Quit()
    {
        SoundManager.instance.PlaySound(interactSound);
        Application.Quit(); //Quits the game (BUILD ONLY)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Ends playmode in editor
#endif
    }
}