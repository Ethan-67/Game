using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* STEERING WHEEL 
 * Steering wheel class rotates steering wheel left and right depending on what side is clicked, and also calls balloon controller methods 
 * to rotate whole balloon. Uses coroutines to do so
 * 
*/
public class SteeringWheel : Interactable
{
    // controller reference 
    private BalloonController balloonController;

    // coroutine 
    private Coroutine steeringCoroutine;

    [SerializeField] PlayerInteract playerInteract;

    private float rotIncrement = 10f;

    private bool turnLeft = false;

    private void Start()
    {
        balloonController = GameObject.Find("HotAirBalloonV4").GetComponent<BalloonController>();
    }

    // called when steering wheel is pressed 
    protected override void Interact()
    {
        // if steering is not already happening create a raycast to find which side of the wheel was clicked and rotate that direction 
        if (steeringCoroutine == null)
        {
            RaycastHit hitInfo = playerInteract.GetHitInfo();

            //Debug.Log("SW pos: " + gameObject.transform.position + " hit point: " + hitInfo.point); 
            if (hitInfo.point.x < gameObject.transform.position.x)
            {
                Debug.Log("Turn Left");
                turnLeft = true;
                balloonController.RotateLeft(); 
            } 
            else
            {
                Debug.Log("Turn Right");
                balloonController.RotateRight(); 
            }
            steeringCoroutine = StartCoroutine(SteeringCoroutine());
            promptMessage = "Steering Wheel Active"; 
        }
        else 
        {
            StopCoroutine(steeringCoroutine);
            balloonController.StopRotation();
            promptMessage = "Steering Wheel";
            steeringCoroutine = null;
            turnLeft = false;
        }
    }

    // coroutine method for steering 
    private IEnumerator SteeringCoroutine()
    {
        while (true)
        {
            // turn left 
            if (turnLeft)
            {
                Debug.Log("Turn Left");
                gameObject.transform.Rotate(gameObject.transform.localRotation.x,
                    gameObject.transform.localRotation.y,
                    gameObject.transform.localRotation.z + rotIncrement);
                //balloonController.RotateLeft();
            }
            else
            {
                Debug.Log("Turn Right"); 
                gameObject.transform.Rotate(gameObject.transform.localRotation.x,
                    gameObject.transform.localRotation.y,
                    gameObject.transform.localRotation.z - rotIncrement);
                //balloonController.RotateRight(); 
            }
            // suspend coroutine 
            yield return new WaitForSeconds(.25f); 
        }
    }
    
}
