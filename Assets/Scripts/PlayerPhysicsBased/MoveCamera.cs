using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* MOVE CAMERA 
 * Moves the players camera to a camera position transform. Necessary as just moving the camera create jitter. 
 * Source https://www.youtube.com/watch?v=cTIAhwlvW9M
*/
public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    Transform cameraPosition;

    // set camera position to transform position 
    private void Update()
    {
        transform.position = cameraPosition.position; 
    }
}
