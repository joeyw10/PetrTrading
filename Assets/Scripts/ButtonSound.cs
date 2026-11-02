
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource soundSource;
    public AudioSource themeSource;
    public AudioSource detectionMusicSource;

    [Header("Sound Effects")]
    public AudioClip buttonSound;
    public AudioClip spawnSound;
    public AudioClip catchSuccessSound;
    public AudioClip catchFailureSound;

    [Header("Music")]
    public AudioClip themeMusic;
    public AudioClip detectionMusic;

    void Start()
    {
        // Set up the general theme music.
        if (themeSource != null && themeMusic != null)
        {
            themeSource.clip = themeMusic;
            themeSource.loop = true;
            themeSource.Play();
        }

        // Set up the detection music, but do not start it yet.
        if (detectionMusicSource != null && detectionMusic != null)
        {
            detectionMusicSource.clip = detectionMusic;
            detectionMusicSource.loop = true;
            detectionMusicSource.Stop();
        }
    }

    public void PlayButtonSound()
    {
        PlaySound(buttonSound);
    }

    public void PlaySpawnSound()
    {
        PlaySound(spawnSound);
    }

    public void PlayCatchSuccessSound()
    {
        PlaySound(catchSuccessSound);
    }

    public void PlayCatchFailureSound()
    {
        PlaySound(catchFailureSound);
    }

    public void PlayDetectionMusic()
{
    if (detectionMusicSource != null && detectionMusic != null)
    {
        if (themeSource != null)
        {
            themeSource.Pause();
        }

        detectionMusicSource.clip = detectionMusic;
        detectionMusicSource.loop = true;
        detectionMusicSource.time = 3f;
        detectionMusicSource.Play();
    }
}
    public void ReturnToMapMusic()
    {
        if (detectionMusicSource != null)
        {
            detectionMusicSource.Stop();
        }

        if (themeSource != null && !themeSource.isPlaying)
        {
            themeSource.UnPause();
        }
    }

    private void PlaySound(AudioClip sound)
    {
        if (soundSource != null && sound != null)
        {
            soundSource.PlayOneShot(sound);
        }
    }
}
