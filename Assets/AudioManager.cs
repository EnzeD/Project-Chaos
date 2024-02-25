using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource nightMusicSource;
    public AudioSource dayMusicSource;

    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    private float originalVolume;

    [Range(0f, 1f)]
    public float sfxVolume = 0.5f; // Volume for sound effects

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dayMusicSource.Play();
        OnGamePaused();
    }
    public void OnGamePaused()
    {
        originalVolume = musicVolume; // Save the current volume
        SetMusicVolume(musicVolume / 2); // Halve the volume
    }

    public void OnGameResumed()
    {
        SetMusicVolume(originalVolume); // Restore the original volume
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        nightMusicSource.volume = volume;
        dayMusicSource.volume = volume;

    }
    public void PlayNightMusic()
    {
        StartCoroutine(FadeMusic(nightMusicSource, dayMusicSource, 1.0f));
    }

    public void PlayDayMusic()
    {
        // Check if dayMusicSource is already playing
        if (!dayMusicSource.isPlaying)
        {
            // Start playing day music with fade in if needed
            StartCoroutine(FadeMusic(dayMusicSource, nightMusicSource, 1.0f));
        }
    }

    IEnumerator FadeMusic(AudioSource fadeInSource, AudioSource fadeOutSource, float duration)
    {
        float currentTime = 0;
        float startFadeOutVolume = fadeOutSource.volume;

        fadeInSource.volume = 0; // Start from silence
        fadeInSource.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadeOutSource.volume = Mathf.Lerp(startFadeOutVolume, 0, currentTime / duration);
            fadeInSource.volume = Mathf.Lerp(0, musicVolume, currentTime / duration);
            yield return null;
        }

        fadeOutSource.Stop();
        fadeOutSource.volume = musicVolume; // Reset volume for next play
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = clip;
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
        Destroy(sfxSource, 1f);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
