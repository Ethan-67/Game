using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLeft : Interactable
{
    [SerializeField] Radio radio; 

    protected override void Interact()
    {
        AudioClip clip = radio.ButtonListener();
        radio.PassNewTrackToRadio(clip);
    }
}
