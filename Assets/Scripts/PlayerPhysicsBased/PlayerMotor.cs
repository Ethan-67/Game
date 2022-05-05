using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PLAYER MOTOR 
 * Handles movement of player, using the players rigid body 
 * Source https://www.youtube.com/watch?v=LqnPeqoJRFY
*/
public class PlayerMotor : MonoBehaviour
{
    // rigid body instance 
    Rigidbody rb;

    Vector3 moveDirection;

    [SerializeField]
    Transform orientation;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float moveMultiplier = 10f;
    [SerializeField] float airMoveMultiplier = 0.4f; 

    [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    [Header("Ground Detection")]
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    float groundDistance = 0.6f;

    float horizontalMovement;
    float verticalMovement;

    float jumpHeight = 15f; 

    float playerHeight = 2f;

    [Header("Gravity")]
    float gravity = 9.2f;

    RaycastHit slopeHit;

    float minDistanceBetweenWalkSounds = 1f;

    AudioSource audioSource;

    [SerializeField] List<AudioClip> footstepClips = new List<AudioClip>(); 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        // updates if player is touching ground 
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down, groundDistance, groundLayer);
    }

    // return bool whether player is on flat ground or not 
    private bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2f + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false; 
    }

    // moves player 
    public void ProcessMove(Vector2 input)
    {
        MoveRigidBody(input);
        ProcessDrag(); 
    }

    // handles jumping for player, this method subscribed to context manager delegate 
    public void Jump()
    {
        if (!isGrounded)
            return;

        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse); 
    }
    
    // moves the players rigid body, depending on circumstance 
    private void MoveRigidBody(Vector2 input)
    {
        Vector3 moveDirection = orientation.forward * input.y + orientation.right * input.x;

        Vector2 oldDirection = new Vector2(orientation.transform.position.x, orientation.transform.position.z);
        // if on ground 
        if (isGrounded && !IsOnSlope())
        {
        rb.AddForce(transform.TransformDirection(moveDirection.normalized) * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if (IsOnSlope()) /* if on slope project vector3 on plane so it will be parallel to plane, move player along this vector3  */
        {
        Vector3 slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        rb.AddForce((transform.TransformDirection(slopeMoveDirection.normalized) * moveSpeed * moveMultiplier) / 100f, ForceMode.Impulse);
        }
        else if (!isGrounded) /* if in air */
        {
            rb.AddForce(transform.TransformDirection(moveDirection.normalized) * moveSpeed * airMoveMultiplier, ForceMode.Acceleration);
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration); 
        }
    }

    // gives a different drag to player depending on if they are in the air or not 
    private void ProcessDrag()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag; 
    }
    
    private void PlayWalkSound()
    {
        audioSource.Play(); 
    }

    private void StopWalkSound()
    {
        audioSource.Stop();
    }
}
