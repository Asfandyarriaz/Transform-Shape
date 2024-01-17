using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour, IInterfaceMovement
{
    [Header("Tank Obstacle Behavior")]
    [SerializeField] VehicleProperties vehicleProperties;
    CharacterController tankCharacterController;
    public bool allowMove = true;

    [Header("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float climbingSpeed;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage = 5f;

    [Header("Gravity Settings")]
    //Gravity
    public Vector3 velocity;
    public float gravity = -5f;

    //Variables
    private float tempForceMultiplier;
    private float tempSpeed;

    private int slowSpeed = 4;
    private float slowDuration = 0.5f;
    private float resetDuration = 1.5f;

    [Header("Slope Rotation")]
    public float rotationSpeed = 45f;  // Adjust the rotation speed as needed
    public float maxSlopeAngle = 45f;  // Adjust the maximum slope angle to consider
    public LayerMask ground;
    public Vector3 offset;
    public float rayCastLength;

    [Header("Tank Object")]
    [SerializeField] GameObject tankObject;

    //Flags
    [Header("Flags")]
    public bool allowRotate = true;
    private bool runOnce = false;


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
            runOnce = false;
        }
        if (state == GameManager.GameState.Play)
        {
            //IncrementSpeed();
        }
    }

    void Start()
    {
        tankCharacterController = GetComponent<CharacterController>();
        speed = vehicleProperties.speed;
        tempSpeed = speed;
    }

    public void Movement()
    {
        if (allowMove)
        {
            tankCharacterController.Move(Vector3.forward * speed * Time.deltaTime);
        }

        if (allowRotate && allowMove)
            RotateToSlope();

        ApplyGravity();
        //Debug.Log("Allow Move " + allowMove);
    }

    //5 % Increment with each level
    void IncrementSpeed()
    {
        if (vehicleProperties.currentUpgradeLevel >= 1 && runOnce != true)
        {
            speed = vehicleProperties.speed;
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel - 1;
            speed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }


    //Reduce speed by 4 times
    public IEnumerator SlowSpeedInDifferentTerrain()
    {
        float time = 0;
        float velocity = 0;
        while (time < slowDuration)
        {
            velocity = Mathf.Lerp(tempSpeed, slowSpeed, time / slowDuration);
            speed = velocity;
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
            velocity = Mathf.Lerp(speed, tempSpeed, time / resetDuration);
            speed = velocity;
            time += Time.deltaTime;
            yield return null;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        tankCharacterController.Move(velocity * Time.deltaTime);
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
                tankObject.transform.rotation = Quaternion.Slerp(tankObject.transform.rotation, slopeRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                tankObject.transform.rotation = Quaternion.Slerp(tankObject.transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            }
        }
    }
    /*public IEnumerator RotateToSlope(string ResetRotate)
    {
        float time = 0;
        float duration = 1f;
        allowRotate = false;
        while (time < duration)
        {
            tankObject.transform.rotation = Quaternion.Slerp(tankObject.transform.rotation, Quaternion.identity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }*/
}