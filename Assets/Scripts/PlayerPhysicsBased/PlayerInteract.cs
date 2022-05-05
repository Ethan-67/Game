using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PLAYER INTERACT 
 * Responsible for handling interacble objects in game 
 * Source https://www.youtube.com/watch?v=gPPGnpV1Y1c&t=1035s
*/
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Transform playerCamera;

    [SerializeField] float rayDistance = 3f;

    [SerializeField] LayerMask interactableMask;

    [SerializeField] PlayerUI playerUI;

    [SerializeField] private InputManager inputManager;

    private RaycastHit hitInfo;

    // if a raycast from the players camera collides with an interactable mask, then call the interables interact method
    private void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);

        if (Physics.Raycast(ray, out hitInfo, rayDistance, interactableMask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage); 
                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }

    public RaycastHit GetHitInfo()
    {
        return hitInfo;
    }
}
