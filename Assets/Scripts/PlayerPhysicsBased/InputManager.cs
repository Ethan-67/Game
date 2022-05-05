using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* INPUT MANAGER 
 * Driver code for calling corrosponding methods when a control is pressed. Uses Unity's new input system for gamepad support eventually. 
 * Source https://www.youtube.com/watch?v=rJqP5EesxLk
*/
public class InputManager : MonoBehaviour
{
    // input controls 
    private PlayerInput playerInput;
    // onfoot actions 
    public PlayerInput.OnFootActions onFoot;

    // movement and camera look script 
    private PlayerMotor motor;
    private PlayerLook look;

    // pause menu instance 
    private PauseMenu pauseMenu;

    //  assign delegates and movement and look scripts 
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = new PlayerInput().OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>(); 

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Pause.performed += ctx => pauseMenu.HandlePause();
        onFoot.Interact.performed += ctx => pauseMenu.HandleClick(); 

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // movement 
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        // camera look 
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
