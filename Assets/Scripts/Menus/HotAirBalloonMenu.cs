using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HOT AIR BALLOON MENU
 * Handles hot air balloon movement as seen in the menu screen 
*/ 
public class HotAirBalloonMenu : MonoBehaviour
{
    private Rigidbody balloonPhysics; 

    private float upBound = 1215f; 
    private float lowBound = 1025f;

    private float upBoundRot = 177f;
    private float lowBoundRot = -10f; 

    private float altitudeMultiplier = 100f;
    private float rotationMultiplier = 0.75f; 

    private bool goDown = false;
    private bool rotateRight = false; 

    private void Start()
    {
        balloonPhysics = gameObject.GetComponent<Rigidbody>(); 
    }

    // Move up if a threshold is met, move down if a threshold is met
    void Update()
    {
        if (goDown) 
        {
            Debug.Log("Go Down");
            //gameObject.transform.position += Vector3.down * altitudeMultiplier;
            balloonPhysics.AddForce(Vector3.down * altitudeMultiplier, ForceMode.Acceleration);
            if (gameObject.transform.position.y < lowBound)
                goDown = false;
            else if (gameObject.transform.position.y > lowBound - 15f)
                balloonPhysics.AddForce(Vector3.up * altitudeMultiplier / 2f, ForceMode.Acceleration);
        }
        else 
        {
            Debug.Log("Go Up"); 
            //gameObject.transform.position += Vector3.up * altitudeMultiplier;
            balloonPhysics.AddForce(Vector3.up * altitudeMultiplier, ForceMode.Acceleration);
            if (gameObject.transform.position.y > upBound)
                goDown = true;
            else if (gameObject.transform.position.y > upBound - 15f)
                balloonPhysics.AddForce(Vector3.down * altitudeMultiplier / 2f, ForceMode.Acceleration);
        }
        // add rotation
        balloonPhysics.AddTorque(Vector3.down * rotationMultiplier, ForceMode.Impulse);
    }
}
