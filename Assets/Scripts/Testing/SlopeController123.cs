using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeController123 : MonoBehaviour
{

    [Header("Speed")]
    public float targetVelocity = 12f;  // Adjust the desired velocity
    public float forceMultiplier = 1f;  // Adjust the force multiplier

    [Header("Slope Rotation")]
    public float rotationSpeed = 45f;  // Adjust the rotation speed as needed
    public float maxSlopeAngle = 45f;  // Adjust the maximum slope angle to consider
    public LayerMask ground;
    public Vector3 offset;
    public float rayCastLength;
    public float stopForceMultiplier = 0.5f;  // Adjust as needed

    [Header("Grounded")]
    public float rayCastGroundLength;
    Rigidbody rb;

    [Header("Air")]
    public float flySpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveForward();
        RotateToSlope();
    }
    void MoveForward()
    {
        /*// Add force to move the object forward
        rb.AddForce(transform.forward * moveSpeed);

        // Optionally, apply a counteracting force to simulate deceleration
        // This helps the object to stop more gradually
        rb.AddForce(-rb.velocity * stopForceMultiplier);*/

        // Calculate the force needed using Newton's second law (Force = Mass * Acceleration)
        // Acceleration = (TargetVelocity - CurrentVelocity) / Time
        //float acceleration = (targetVelocity - rb.velocity.magnitude) / Time.fixedDeltaTime;
        // If the object is not grounded, apply a fraction of the force to avoid excessive forces in the air

        if (IsGrounded())
        {
            forceMultiplier = 1;

            float acceleration = (targetVelocity - rb.velocity.magnitude) / Time.fixedDeltaTime;
            //acceleration *= 0.1f; // Adjust the fraction as needed
            // Calculate force needed
            float force = rb.mass * acceleration;

            // Add force in the forward direction
            rb.AddForce(transform.forward * forceMultiplier * force);
        }
        else
        {
            forceMultiplier = 0;
            rb.AddForce(Vector3.forward * flySpeed);
        }

        /*// Calculate force needed
        float force = rb.mass * acceleration;

        // Add force in the forward direction
        rb.AddForce(transform.forward * forceMultiplier * force);*/
    }

    void RotateToSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + offset, Vector3.down, out hit, rayCastLength, ground))
        {
            Debug.DrawRay(transform.position + offset, Vector3.down * rayCastLength, Color.yellow);
            // Check if the surface hit is a slope (based on the angle)
            if (Vector3.Angle(hit.normal, Vector3.up) > 0 && Vector3.Angle(hit.normal, Vector3.up) < maxSlopeAngle)
            {
                // Calculate the rotation based on the slope normal
                Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                // Rotate the object smoothly
                transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    bool IsGrounded()
    {
        // Implement your own ground check logic here
        // You can use raycasting or other methods to check if the object is grounded
        // For simplicity, you can use a simple raycast in this example
        if (Physics.Raycast(transform.position, Vector3.down, rayCastGroundLength))
        {
            Debug.DrawRay(transform.position, Vector3.down * rayCastGroundLength, Color.red);
            return true;
        }
        return false;
    }
}
