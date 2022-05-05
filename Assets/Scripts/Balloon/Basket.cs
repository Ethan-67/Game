using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BASKET 
 * Encapsulates the basket of the hot air balloon 
*/
public class Basket : MonoBehaviour
{
    Rigidbody basketPhysics;

    BalloonController controller; 
    float rotationValue = 0.5f; 

    // Start is called before the first frame update
    void Start()
    {
        basketPhysics = gameObject.GetComponent<Rigidbody>();
        //basketPhysics.freezeRotation = true;
        controller = GameObject.Find("HotAirBalloonV4").GetComponent<BalloonController>();
    }

    /*
    private void FixedUpdate()
    {
        if (controller.GetTurnLeft())
        {
            //basketPhysics.freezeRotation = false;
            float yRot = gameObject.transform.rotation.y + rotationValue * Time.fixedDeltaTime;

            Quaternion rot = Quaternion.Euler(gameObject.transform.rotation.x, yRot, gameObject.transform.rotation.z);
            gameObject.transform.rotation = rot;
            //gameObject.transform.position = rot;
        }
        else if (controller.GetTurnRight())
        {
            //basketPhysics.freezeRotation = false;
            float yRot = gameObject.transform.rotation.y - rotationValue * Time.fixedDeltaTime;

            Quaternion rot = Quaternion.Euler(gameObject.transform.rotation.x, yRot, gameObject.transform.rotation.z);
            gameObject.transform.rotation = rot;
        }
        //else
            //basketPhysics.freezeRotation = true;
    }
    */
}
