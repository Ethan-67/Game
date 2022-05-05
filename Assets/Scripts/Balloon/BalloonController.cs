using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BALLOON CONTROLLER 
 * Balloon controller is responsible for calling all revelant method in the balloon and handles all interactions between 
 * interactables and balloon controls. 
*/
public class BalloonController : MonoBehaviour
{
    // different elements of balloon 
    private Balloon balloon;
    private Basket basket;
    private HotAirGauge hotAirGauge;
    private BalloonSoundEffects balloonSoundEffects; 

    // coroutines for releasing and creating hot air 
    private Coroutine burnerCoroutine;
    private Coroutine releaseCoroutine;

    private Vector3 forwardDirection; 

    float rotationValue = 1.5f;

    private bool burnerActive;

    private bool turnLeft = false;
    private bool turnRight = false;

    // Start is called before the first frame update
    void Start()
    {
        balloon = GameObject.Find("Balloon").GetComponent<Balloon>(); 
        basket = GameObject.Find("Basket").GetComponent<Basket>();
        hotAirGauge = GameObject.Find("HotAirGauge").GetComponent<HotAirGauge>();
        balloonSoundEffects = GameObject.Find("BalloonSoundEffects").GetComponent<BalloonSoundEffects>(); 
        forwardDirection = Vector3.forward * 0.3f; 
    }

    private void FixedUpdate()
    {
        if (turnLeft)
        {
            float lastRot = gameObject.transform.rotation.y;
            float yRot = lastRot - rotationValue * Time.fixedDeltaTime;

            Quaternion rot = Quaternion.Euler(gameObject.transform.rotation.x, yRot, gameObject.transform.rotation.z);
            //gameObject.transform.rotation = rot;
            gameObject.transform.Rotate(rot.eulerAngles);
        }
        else if (turnRight)
        {
            float lastRot = gameObject.transform.rotation.y;
            float yRot = lastRot + rotationValue * Time.fixedDeltaTime;

            Quaternion rot = Quaternion.Euler(gameObject.transform.rotation.x, yRot, gameObject.transform.rotation.z);
            //gameObject.transform.rotation = rot;
            gameObject.transform.Rotate(rot.eulerAngles);
        }
    }

    // called from burner rope class, flips the bool to specify if the burner is active or not
    public void HandleBurner(bool isActive)
    {
        // if active start coroutine to start hot air 
        if (isActive)
        {
            burnerActive = true; 
            burnerCoroutine = StartCoroutine(StartBurner());
            balloonSoundEffects.PlayBurnerStart(); 
        }
        else /* otherwise stop coroutine */
        {
            burnerActive = false; 
            StopCoroutine(burnerCoroutine);
            balloonSoundEffects.StopAudioSource();
        }
    }

    // coroutine method for starting burner all methods are called again until next interval or coroutine stopped 
    private IEnumerator StartBurner()
    {
        while (true)
        {
            //increase altitude 
            balloon.IncreaseAltitude();

            if (!balloonSoundEffects.IsPlaying())
            {
                balloonSoundEffects.PlayBurnerLoop(); 
            }
            // HARD CODED UP BOUND HOT AIR ****
            if (GetHotAir() < 125f)
            {
                // handle gauges inside balloon 
                hotAirGauge.RotateNeedlePositive();
            }
            // suspend for now 
            yield return new WaitForSeconds(0.24f);
        }
    }

    // handles release 
    public void HandleRelease(bool isActive)
    {
        if (isActive)
        {
            releaseCoroutine = StartCoroutine(StartRelease());
            balloonSoundEffects.PlayReleaseLoop(); 
        }
        else
        {
            StopCoroutine(releaseCoroutine);
            balloonSoundEffects.StopAudioSource();
        }
    }

    // coroutine method for handling release of hot air 
    private IEnumerator StartRelease()
    {
        while (true)
        {
            // decrease altitude 
            balloon.DecreaseAltitude();
            // HARD CODED LOW BOUND HOT AIR ***** 
            if (GetHotAir() > -100f)
            {
                // handle gauges inside balloon
                hotAirGauge.RotateNeedleNegative(); 
            }
            // suspend coroutine for now
            yield return new WaitForSeconds(0.24f);
        }
    }

    public bool isBurnerActive()
    {
        return burnerActive; 
    }

    public float GetHotAir()
    {
        return balloon.GetHotAir(); 
    }

    public void RotateLeft()
    {
        turnLeft = true;
    }

    public void RotateRight()
    {
        turnRight = true;
    }

    public void StopRotation()
    {
        turnLeft = false;
        turnRight = false;
    }

    public bool GetTurnLeft()
    {
        return turnLeft;
    }

    public bool GetTurnRight()
    {
        return turnLeft;
    }

    /*
    public void RotateLeft()
    {
        Vector3 newDirection = Vector3.down * rotationValue;

        gameObject.transform.Rotate(newDirection);
        //basket.ApplyRotation(forwardDirection + newDirection);
    }

    public void RotateRight()
    {
        Vector3 newDirection = Vector3.up * rotationValue;

        gameObject.transform.Rotate(newDirection); 
    }
    */


}
