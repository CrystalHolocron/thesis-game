using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    private CharacterController characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizonatlInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizonatlInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * movementSpeed; // Ensures less movement if joypad stick is not fully tilted
        movementDirection.Normalize(); // Locks movementDirection vector to 1 so diagonal movement isn't faster than ordinal movement

        // transform.Translate(movementDirection * magnitude * Time.deltaTime, Space.World); // movement
        characterController.SimpleMove(movementDirection * magnitude); // movement using the CharacterController

        if (movementDirection != Vector3.zero)
        {
            // transform.forward = movementDirection; // instantly snaps character to face movementDirection

            Quaternion toRotate = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }
    }
}
