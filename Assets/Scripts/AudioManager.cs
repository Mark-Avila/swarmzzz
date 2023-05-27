using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource effectSource2d;
    [SerializeField] private AudioSource effectSource3d;
    [SerializeField] private Transform listener;

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

    //public void FixedUpdate()
    //{
    //    this.transform.position = listener.position;
    //}

    public void PlayAudio2d(AudioClip clip)
    {
        effectSource2d.PlayOneShot(clip);
    }
    public void PlayAudio3d(AudioClip clip)
    {
        effectSource3d.PlayOneShot(clip);
    }
}
