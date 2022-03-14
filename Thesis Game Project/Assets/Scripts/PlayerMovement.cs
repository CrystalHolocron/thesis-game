using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;
    public float gravitySpeed;

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * movementSpeed; // Ensures less movement if joystick is not fully tilted
        movementDirection.Normalize(); // Locks movementDirection vector to 1 so diagonal movement isn't faster than ordinal movement

        ySpeed += Physics.gravity.y * gravitySpeed * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }

        // Jump Mechanic w/o "Coyote Time"
        //if (characterController.isGrounded)
        //{
        //    characterController.stepOffset = originalStepOffset;
        //    ySpeed = -0.5f;
        //    if (Input.GetButtonDown("Jump"))
        //    {
        //        ySpeed = jumpSpeed;
        //    }
        //}
        //else
        //{
        //    characterController.stepOffset = 0;
        //}

        // transform.Translate(movementDirection * magnitude * Time.deltaTime, Space.World); // movement
        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime); // movement using the CharacterController

        if (movementDirection != Vector3.zero)
        {
            // transform.forward = movementDirection; // instantly snaps character to face movementDirection

            Quaternion toRotate = Quaternion.LookRotation(movementDirection, Vector3.up);
            toRotate *= Quaternion.Euler(0, -90, 0); // Adds a -90/270 degree Y rotation to account for incorrect toRotate angle

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }
    }
}
