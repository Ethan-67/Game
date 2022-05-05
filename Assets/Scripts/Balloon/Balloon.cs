using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BALLOON 
 * Balloon class responsible for adding altitude to the hot air balloon 
 * 
 * 
*/
public class Balloon : MonoBehaviour
{
    // component references 
    public Rigidbody balloonPhysics;
    public HingeJoint hinge;
    // controller reference 
    private BalloonController balloonController;

    private Vector3 forwardDirection;

    // balloon properties 
    private float moveSpeed = 0.3f;

    private float hotAir = 0f;
    private float yVeloMultiplier = 4.25f;

    private float hotAirIncreaseRate = 1f;
    private float hotAirDecreaseRate = 0.5f;

    private float hotAirLowBound = -100f;
    private float hotAirUpBound = 125f;

    // initialise 
    private void Start()
    {
        balloonPhysics = gameObject.GetComponent<Rigidbody>();
        hinge = gameObject.GetComponent<HingeJoint>();
        balloonController = GameObject.Find("HotAirBalloonV4").GetComponent<BalloonController>();
        //forwardDirection = Vector3.forward * 0.4f; 
        forwardDirection = GameObject.Find("HotAirBalloonV4").transform.forward * 0.8f;
    }

    // called every frame, if hot air below a threshold then apply a upward force, if threshold reached also apply a forward force 
    private void FixedUpdate()
    {
        if (hotAir > 105f)
        {
            balloonPhysics.AddForce(forwardDirection, ForceMode.VelocityChange);
        }
        balloonPhysics.AddForce(Vector3.up * hotAir * yVeloMultiplier, ForceMode.Acceleration);
    }

    // increase hot air 
    public void IncreaseAltitude()
    {
        Debug.Log("B Hot Air: " + hotAir); 
        hotAir += hotAirIncreaseRate;
        hotAir = Mathf.Clamp(hotAir, hotAirLowBound, hotAirUpBound);
    }

    // decrease hot air 
    public void DecreaseAltitude()
    {
        Debug.Log("R Hot Air: " + hotAir);
        hotAir -= hotAirDecreaseRate;
        hotAir = Mathf.Clamp(hotAir, hotAirLowBound, hotAirUpBound);
    }

    public void AddSway()
    {

    } 

    public float GetHotAir()
    {
        return hotAir; 
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
