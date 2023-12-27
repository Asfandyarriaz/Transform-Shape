using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] public VehicleProperties vehicleProperties;
    [SerializeField] public bool allowMove;
    private float downSpeed;
    Rigidbody rb;
    [Header("Speed")]
    [SerializeField] private float speed;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage;
    //Flags
    [SerializeField] public bool forceEffect = false;
    [SerializeField] private float waitTimerForForceEffect;
    private bool runOnce = false;

    //Variables
    BikeObstacleBehaviour bikeObstacleBehaviour;

    private void OnEnable()
    {
        bikeObstacleBehaviour.enabled = true;
    }
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
        if (state == GameManager.GameState.Cash)
        {
            bikeObstacleBehaviour.enabled = false;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        downSpeed = vehicleProperties.speed;
        bikeObstacleBehaviour = GetComponent<BikeObstacleBehaviour>();
    }

    public void Movement()
    {
            if (allowMove)
            {
                rb.velocity = Vector3.forward * vehicleProperties.speed;
            }
            else
            {
                rb.velocity = new Vector3(0, -downSpeed, 0);
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
}
