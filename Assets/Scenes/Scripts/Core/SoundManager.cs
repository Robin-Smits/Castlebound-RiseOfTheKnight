using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {

        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //Keep this object even when we go to new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Destroy duplicate gameobjects
        else if (instance != null && instance != this)
            Destroy(gameObject);

        //Assign initial volume
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }
    // Plays the given sound
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    // Changes the volume of the gamesounds by the given amount
    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }

    //// Changes the volume of the music by the given amount
    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(0.3f, "musicVolume", _change, musicSource);
    }

    // Manages the volume of the given sound source
    private void ChangeSourceVolume(
        float baseVolume,
        string volumeName,
        float change,
        AudioSource source
    )
    {
        //Get initial value
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        //Check min/max value
        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        //Assign final value
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        //Save Final Value
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}