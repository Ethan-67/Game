using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PLAYER LOOK 
 * Responsible for moving the player camera in correlation with mouse inputs 
 * https://www.youtube.com/watch?v=E5zNi_SSP_w
*/
public class PlayerLook : MonoBehaviour
{
    // camera 
    [SerializeField]
    Transform cam;
    [SerializeField]
    Transform orientation;

    // sensitivities 
    [SerializeField]
    float xSens;
    [SerializeField]
    float ySens;

    float mouseX;
    float mouseY;

    float multiplier = 0.6f;

    float xRot;
    float yRot;

    bool isActive = true; 

    // set mouse to be locked on centre of screen 
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // moves camera takes in mouse vector2 direction 
    public void ProcessLook(Vector2 input)
    {
        if (isActive)
        {
            MoveCamera(input);
        }
    }

    private void MoveCamera(Vector2 input)
    {
        // map mouse x y
        float mouseX = input.x;
        float mouseY = input.y;

        // cal camera rot
        yRot += mouseX * xSens * multiplier;
        xRot -= mouseY * ySens * multiplier;

        // clamp so camera will not rotate too far 
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        // rotate 
        cam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);

        // rotate 
        //transform.Rotate(Vector3.up * yRot);
        orientation.transform.rotation = Quaternion.Euler(0, yRot, 0); 
    }

    public void SetActive(bool active)
    {
        isActive = active; 
    }

    // flips mouse lock state used for when menus are active so player can use mouse to use them 
    public void SetMouseLockedState(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
