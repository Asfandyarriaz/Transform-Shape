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

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage;
    //Flags
    [SerializeField] public bool forceEffect = false;
    [SerializeField] private float waitTimerForForceEffect;
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
}
