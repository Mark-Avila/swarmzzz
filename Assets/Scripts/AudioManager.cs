using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Transform listener;

    //Used for one-shot audio (eg. gun sounds, damage sounds)
    [SerializeField] private AudioSource quickAudio;
    [SerializeField] private AudioClip bgm;

    //Used for handling multiple long loop audio (zombie sound effects)
    private List<AudioSource> audioSources = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayAudioClip(bgm, 0.05f);
    }

#nullable enable
    public void PlayAudioClip(AudioClip clip, float? volume)
    {
        bool isAlreadyPlaying = false;

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == clip)
                isAlreadyPlaying = true;
        }

        if (isAlreadyPlaying)
            return;

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = clip;
        newAudioSource.loop = true;
        newAudioSource.volume = volume ?? 0.25f;
        newAudioSource.Play();

        audioSources.Add(newAudioSource);
    }

    public void StopAudioClip(AudioClip clip)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == clip)
            {
                audioSource.Stop();
                audioSources.Remove(audioSource);
                Destroy(audioSource);
                break;
            }
        }
    }

    public void StopAllAudioClips()
    {
        // Stop and destroy all AudioSources
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
            Destroy(audioSource);
        }

        // Clear the list
        audioSources.Clear();
    }

    public void PlayQuickAudio(AudioClip clip)
    {
        quickAudio.PlayOneShot(clip);
    }

    public void SetBGMAudio(float newVolume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == bgm)
            {
                audioSource.volume = newVolume;
                break;
            }
        }
    }

    public void StopQuickAudio()
    {
        quickAudio.Stop();
    }
}
