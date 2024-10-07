using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    [Header ("Sounds")]
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioClip changeSound;

    //Loads level select scene
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SoundVolume() {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume() {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

        public void Quit()
    {
        Application.Quit(); //Quits the game (BUILD ONLY)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Ends playmode in editor
#endif
    }
}