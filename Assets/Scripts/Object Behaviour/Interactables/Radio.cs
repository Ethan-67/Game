using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : Interactable
{
    GameObject radio;

    GameObject buttonLeft;
    GameObject buttonRight;

    AudioSource audioSource;

    [SerializeField] List<AudioClip> radioTracks = new List<AudioClip>();
    [SerializeField] List<AudioClip> staticTracks = new List<AudioClip>();

    int staticTrackChanceMultiplier = 6; 

    bool isActive = false;

    private void Start()
    {
        radio = gameObject;
        buttonLeft = GameObject.Find("Button Left");
        buttonRight = GameObject.Find("Button Right");
        buttonLeft.SetActive(false);
        buttonRight.SetActive(false);
        audioSource = gameObject.GetComponentInParent<AudioSource>();
        audioSource.Stop();
    }

    protected override void Interact()
    {
        isActive = !isActive; 

        if (isActive)
        {
            HandleActiveState();

            audioSource.clip = GetStatic();
            audioSource.Play(); 
        }
        else
        {
            HandleInActiveState();
            audioSource.Stop();
        }
    }

    private void HandleActiveState()
    {
        promptMessage = "Radio Active";
        buttonLeft.SetActive(true);
        buttonRight.SetActive(true);
    }

    private void HandleInActiveState()
    {
        promptMessage = "Radio";
        buttonLeft.SetActive(false);
        buttonRight.SetActive(false);
    }

    public AudioClip ButtonListener()
    {
        // random int 0 - 5 (minInclusice - maxExclusive)
        int chance = Random.Range(0, 6);

        // 40% chance to play a song 
        if (chance > 3)
        {
            int randSong = Random.Range(0, radioTracks.Count);
            return radioTracks[randSong]; 
        }
        return GetStatic(); 
    }

    public void PassNewTrackToRadio(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play(); 
    }

    private AudioClip GetStatic()
    {
        return staticTracks[0];
    }
}
