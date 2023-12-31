using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMovement : MonoBehaviour, IInterfaceMovement
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


    [Header("Grounded")]
    public float rayCastGroundLength;
    Rigidbody rb;

    [Header("Air")]
    public float flySpeed;

    //Flags
    [Header("Flags")]
    public bool allowMove;

    [Header("Other Settings")]
    [SerializeField] VehicleProperties vehicleProperties;

    //Variables
    private float tempForceMultiplier;
    private float tempTargertVelocity;

    private int slowSpeed = 4;
    private float slowDuration = 0.5f;
    private float resetDuration = 1.5f;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Start)
        {
            //runOnce = false;
        }
        if (state == GameManager.GameState.Play)
        {
            //targetVelocity = vehicleProperties.speed;
            //IncrementSpeed();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempForceMultiplier = forceMultiplier;
        tempTargertVelocity = targetVelocity;
    }
    public void Movement()
    {
        if (allowMove)
        {
            MoveForward();
            RotateToSlope();
        }
    }
    void MoveForward()
    {
        if (IsGrounded())
        {
            forceMultiplier = tempForceMultiplier;
            float acceleration = (targetVelocity - rb.velocity.magnitude) / Time.fixedDeltaTime;
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
        if (Physics.Raycast(transform.position, Vector3.down, rayCastGroundLength))
        {
            Debug.DrawRay(transform.position, Vector3.down * rayCastGroundLength, Color.red);
            return true;
        }
        return false;
    }

    //Reduce speed by 4 times
    public IEnumerator SlowSpeedInWater()
    {
        float time = 0;
        float velocity = 0;
        while (time < slowDuration)
        {
            velocity = Mathf.Lerp(targetVelocity, slowSpeed, time / slowDuration);
            targetVelocity = velocity;
            time += Time.deltaTime;
            yield return null;
        }

    }
    public IEnumerator ResetSpeed()
    {
        float time = 0;
        float velocity = 0;
        while (time < resetDuration)
        {
            velocity = Mathf.Lerp(targetVelocity, tempTargertVelocity, time / resetDuration);
            targetVelocity = velocity;
            time += Time.deltaTime;
            yield return null;
        }
    }
}
