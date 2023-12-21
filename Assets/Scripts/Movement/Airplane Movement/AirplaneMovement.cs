using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] BezierFollow bezierFollowScript;

    [Header("Set above the ground")]
    [SerializeField] float yOffset;
    [Header("Speed")]
    [SerializeField] private float speed;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage = 5f;

    //Variables
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
            runOnce = true;
        }
        if (state == GameManager.GameState.Play)
        {
            speed = vehicleProperties.speed;
            IncrementSpeed();
        }
    }

    private void Start()
    {
        bezierFollowScript.speedModifier = vehicleProperties.speed;
    }

    public void Movement()
    {
        if (!bezierFollowScript.bezierRunning)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                Vector3 newPosition = hit.point + new Vector3(0, yOffset, 0);
                transform.position = newPosition;
            }
        }
    }
    //5 % Increment with each level
    void IncrementSpeed()
    {
        if (vehicleProperties.currentUpgradeLevel > 1 && runOnce != true)
        {
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel;
            speed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }
}
