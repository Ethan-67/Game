using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BURNER ROPE 
 * Handles when the burner is active on balloon or not 
*/
public class BurnerRope : Interactable
{
    private BalloonController controller;

    private bool burnerActive = false;

    private void Start()
    {
        controller = GameObject.Find("HotAirBalloonV4").GetComponent<BalloonController>(); 
    }

    // called when burner rope is clicked 
    protected override void Interact()
    {
        // flip active state 
        burnerActive = !burnerActive;
        // send active state to controller and update message displayed of burner rope 
        if (burnerActive)
        {
            Debug.Log("Interacted with: " + gameObject.name);
            promptMessage = "Burner Rope Active"; 
            controller.HandleBurner(burnerActive);
        }
        else
        {
            promptMessage = "Burner Rope";
            controller.HandleBurner(burnerActive);
        }
    }
}
