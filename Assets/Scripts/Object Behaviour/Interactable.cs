using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* INTERACTABLE 
 * Interactable abstract class is what all interactable objects are derived from 
 * Source https://www.youtube.com/watch?v=gPPGnpV1Y1c
*/
public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    
    public void BaseInteract()
    {
        Interact(); 
    }

    protected virtual void Interact()
    {
        //Debug.Log("Interact Base Class"); 
    }
}
