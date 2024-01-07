using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gliderNewMovementTest : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController characterController;
    private Vector3 velocity;


    private float tempSpeed;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        tempSpeed = speed;
    }

    void Update()
    {
        HandleMovementInput();
        ApplyGravity();

        // Apply the final movement to the character controller
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleMovementInput()
    {
        // Get horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        Vector3 movement = new Vector3(0, 0f, 1);
        movement = transform.TransformDirection(movement);
        movement *= speed;

        // Apply movement to velocity
        velocity.x = movement.x;
        velocity.z = movement.z;

        // Jumping
        if (characterController.isGrounded)
        {
            velocity.y = -0.5f; // Small downward force to keep the character grounded

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2f * jumpForce * gravity);
            }
        }
        if (characterController.isGrounded)
        {
            speed = 2.5f;
        }
        else
        {
            speed = tempSpeed;
        }
    }

    void ApplyGravity()
    {
        // Apply gravity to velocity
        velocity.y -= gravity * Time.deltaTime;
    }
}
