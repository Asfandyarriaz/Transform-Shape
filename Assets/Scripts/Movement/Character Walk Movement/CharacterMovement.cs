using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    CharacterController characterController;
    public bool allowMove;
    public bool isOnGround;
    public bool isClimbable;

    [Header("Speed")]
    [SerializeField] private float speed;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage = 5f;

    //Flags
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

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Movement()
    {
        if (allowMove)
        {
            if (isClimbable)
            {
                characterController.Move(Vector3.up * speed * Time.deltaTime);
            }
            else
            {
                characterController.Move(Vector3.forward * speed * Time.deltaTime);
            }
        }
        else
        {
            characterController.Move(Vector3.down * speed * Time.deltaTime);
        }
        if(!isOnGround && !isClimbable) { characterController.Move(Vector3.down * vehicleProperties.speed * Time.deltaTime); }
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
