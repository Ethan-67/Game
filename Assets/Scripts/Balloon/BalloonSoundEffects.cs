using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSoundEffects : MonoBehaviour
{
    [SerializeField] List<AudioClip> sounds = new List<AudioClip>();

    AudioSource audioSource; 

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayBurnerStart()
    {
        audioSource.loop = false;
        audioSource.clip = sounds[0];
        audioSource.Play(); 
    }

    public void PlayBurnerLoop()
    {
        audioSource.loop = true;
        audioSource.clip = sounds[1];
        audioSource.Play(); 
    }

    public void PlayReleaseLoop()
    {
        audioSource.loop = true;
        audioSource.clip = sounds[2];
        audioSource.Play();
    }

    public void StopAudioSource()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying; 
    }
}
