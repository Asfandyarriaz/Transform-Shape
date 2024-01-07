using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] public bool allowMove;
    Rigidbody rb;
    [Header("Speed")]
    [SerializeField] private float speed;

    [Header("Airplane Movement Setting")]
    [SerializeField] public float hoverHeight = 2f;      // Desired height above the ground
    [SerializeField] private float forwardSpeed = 2f;
    [SerializeField] private float maxFloatHeightFactor = 2f; // Factor to adjust maxFloatHeight dynamically
    [SerializeField] private float floatSpeed = 2f;       // Speed of ascending and descending
    [SerializeField] private float wallDetectionDistance = 3f; // Distance to detect walls in front
    [SerializeField] Vector3 raycastOffset;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage;
    //Flags
    [SerializeField] public bool forceEffect = false;
    [SerializeField] private float waitTimerForForceEffect;

    private bool moveForward = true;
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
            IncrementSpeed();
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Movement()
    {
        if (allowMove)
        {
            rb.AddForce((Time.deltaTime * Vector3.forward) * vehicleProperties.speed, ForceMode.Impulse);
            rb.velocity = (Vector3.forward * vehicleProperties.speed);
            StayAboveGround();
        }
        else
        {
            rb.velocity = (Vector3.down * vehicleProperties.speed);
        }
    }
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
          
            // Gradually ascend or descend the object to the target height
            float newY = Mathf.Lerp(transform.position.y, targetHeight, Time.deltaTime * floatSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            /*if (moveForward)
            {
                //Move Forward
                transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
            }*/
        }
    }
}
