using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;

    [Header("Airplane Setting")]
    public float forwardSpeed = 10f;
    public float heightAdjustmentSpeed = 5f;
    public float raycastDistance = 5f;
    public LayerMask groundLayer;
    public float heightOffset = 0.1f;


    Rigidbody rb;
    //Flags
    public bool allowMove = true;
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

        }
        if (state == GameManager.GameState.Play)
        {
            //IncrementSpeed();
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
    /*void IncrementSpeed()
    {       
        if (vehicleProperties.currentUpgradeLevel >= 1 && runOnce != true)
        {
            forwardSpeed = vehicleProperties.speed;
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel - 1;
            forwardSpeed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }*/

    //TODO: Hardcoded Values Fix
    void StayAboveGround()
    {       
        if (allowMove)
        {
            forwardSpeed = Mathf.Lerp(forwardSpeed, 12, 1 * Time.fixedDeltaTime);
            Vector3 forwardVelocity = transform.forward * forwardSpeed;
            rb.velocity = forwardVelocity;
            Debug.Log("Forward Speed : " + forwardSpeed);
        }
        else
        {
            rb.velocity = Vector3.zero;
            forwardSpeed = 0;
        }

        // Raycast to detect the ground below
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, groundLayer))
        {
            Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.green);
            // Adjust the height of the airplane with an offset
            float targetHeight = hit.point.y + heightOffset;

            float currentHeight = transform.position.y;
            float newHeight = Mathf.Lerp(currentHeight, targetHeight, heightAdjustmentSpeed * Time.fixedDeltaTime);

            // Set the new height
            Vector3 newPosition = new Vector3(transform.position.x, newHeight, transform.position.z);
            rb.MovePosition(newPosition);
        }
    }   
}

