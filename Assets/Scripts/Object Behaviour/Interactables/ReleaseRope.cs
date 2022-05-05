using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* RELEASE ROPE
 * Handles releasing hot air 
*/ 
public class ReleaseRope : Interactable
{
    private bool releaseActive = false;
    // controller reference 
    private BalloonController controller; 

    void Start()
    {
        controller = GameObject.Find("HotAirBalloonV4").GetComponent<BalloonController>(); 
    }

    // called when release rope is clicked 
    protected override void Interact()
    {
        // flip activity state 
        releaseActive = !releaseActive;
        // pass active state to controller to handle releasing hot air 
        if (releaseActive)
        {
            Debug.Log("Interacted with: " + gameObject.name);
            promptMessage = "Release Rope Active"; 
            controller.HandleRelease(releaseActive);
        }
        else
        {
            promptMessage = "Release Rope";
            controller.HandleRelease(releaseActive);
        }
    }
}
