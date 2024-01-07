using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;

    [Header("Set above the ground")]
    [SerializeField] float yOffset;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage = 5f;

    [Header("Airplane Movement Setting")]
    [SerializeField] private float hoverHeight = 2f;      // Desired height above the ground
    [SerializeField] private float forwardSpeed = 2f; 
    [SerializeField] private float maxFloatHeightFactor = 2f; // Factor to adjust maxFloatHeight dynamically
    [SerializeField] private float floatSpeed = 2f;       // Speed of ascending and descending
    [SerializeField] private float wallDetectionDistance = 3f; // Distance to detect walls in front
    [SerializeField] Vector3 raycastOffset;
    private Rigidbody rb;

    //Flags
    private bool runOnce = false;
    private bool moveForward = true;
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
            IncrementSpeed();
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Movement()
    {
        StayAboveGround();
    }
    //5 % Increment with each level
    void IncrementSpeed()
    {       
        if (vehicleProperties.currentUpgradeLevel >= 1 && runOnce != true)
        {
            forwardSpeed = vehicleProperties.speed;
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel - 1;
            forwardSpeed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }
    void StayAboveGround()
    {
        // Cast a ray downward from the object's position
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            // Calculate the target height above the ground
            float targetHeight = hit.point.y + hoverHeight;

            // Calculate the maximum allowed height dynamically based on the ground beneath
            float maxFloatHeight = hit.point.y + hoverHeight * maxFloatHeightFactor;

            // Gradually bring the object down if it exceeds the dynamically calculated maximum allowed height
            if (targetHeight > maxFloatHeight)
            {
                targetHeight = Mathf.Lerp(transform.position.y, maxFloatHeight, Time.deltaTime * floatSpeed);
            }

            // Check for a wall in front
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit wallHit, wallDetectionDistance))
            {
                if (!wallHit.collider.CompareTag("Win"))
                {
                    moveForward = false;

                    // If a wall is detected, limit the forward movement until the object is above the wall
                    float wallHeight = wallHit.point.y + hoverHeight;

                    // Only move forward if the object is above the height of the wall
                    if (transform.position.y >= wallHeight)
                    {
                        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
                    }

                    // Gradually ascend the object to the height of the wall
                    targetHeight = Mathf.Max(targetHeight, wallHeight);

                    StartCoroutine(KeepMovingUp());
                }
            }

            // Gradually ascend or descend the object to the target height
            float newY = Mathf.Lerp(transform.position.y, targetHeight, Time.deltaTime * floatSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (moveForward)
            {
                //Move Forward
                transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
            }
        }
    }
    IEnumerator KeepMovingUp()
    {       
        //float newY = Mathf.Lerp(transform.position.y, transform.position.y + 50f, Time.deltaTime * floatSpeed);
        yield return new WaitForSeconds(1.5f);
        moveForward = true;
    }
}
