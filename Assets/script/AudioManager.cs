using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    public AudioClip cardAcquire;

    [Header("Music")]
    public AudioClip day0Music;
    public AudioClip day1MorningMusic;
    public AudioClip day1NoonMusic;
    public AudioClip day1NightMusic;

    [Header("Special Music")]
    public AudioClip reasoningBoardMusic;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private AudioClip currentNormalMusic;  // remembers background music

    private Coroutine musicFadeRoutine;
    public float musicFadeDuration = 1.5f;

    private bool isOverridePlaying = false;
    private AudioClip overrideMusic;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.clip = day0Music;
        musicSource.Play();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        currentNormalMusic = clip;

        // If override is active, DO NOT interrupt
        if (isOverridePlaying) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);

        musicFadeRoutine = StartCoroutine(CrossfadeMusic(clip));
    }

    public void PlayReasoningMusic()
    {
        if (reasoningBoardMusic == null) return;

        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);

        musicFadeRoutine = StartCoroutine(CrossfadeMusic(reasoningBoardMusic));
    }

    public void ResumeNormalMusic()
    {
        if (isOverridePlaying)
        {
            PlayOverrideMusic(overrideMusic);
        }
        else if (currentNormalMusic != null)
        {
            PlayMusic(currentNormalMusic);
        }
    }

    public void PlayOverrideMusic(AudioClip clip)
    {
        if (clip == null) return;

        isOverridePlaying = true;
        overrideMusic = clip;

        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);

        musicFadeRoutine = StartCoroutine(CrossfadeMusic(clip));
    }

    public void StopOverrideMusic()
    {
        if (!isOverridePlaying) return;

        isOverridePlaying = false;

        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);

        musicFadeRoutine = StartCoroutine(CrossfadeMusic(currentNormalMusic));
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        float startVolume = musicSource.volume;

        // Fade out
        float t = 0f;
        while (t < musicFadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / musicFadeDuration);
            yield return null;
        }

        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        t = 0f;
        while (t < musicFadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, t / musicFadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}