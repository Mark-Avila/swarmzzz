using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Transform listener;

    //Used for one-shot audio (eg. gun sounds, damage sounds)
    [SerializeField] private AudioSource quickAudio;
    
    //Used for handling multiple long loop audio (zombie sound effects)
    private List<AudioSource> audioSources = new();

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

    public void PlayAudioClip(AudioClip clip)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = clip;
        newAudioSource.loop = true;
        newAudioSource.volume = 0.25f;
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


    //public void FixedUpdate()
    //{
    //    this.transform.position = listener.position;
    //}

    public void PlayQuickAudio(AudioClip clip)
    {
        quickAudio.PlayOneShot(clip);
    }
}
