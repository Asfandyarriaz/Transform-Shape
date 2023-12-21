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
    PlayerData playerData;

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
            playerData = SaveManager.Instance.LoadData();
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
        rb = GetComponent<Rigidbody>();
        downSpeed = vehicleProperties.speed;
    }

    public void Movement()
    {
            if (allowMove)
            {
                rb.velocity = Vector3.forward * speed;
            }
            else
            {
                rb.velocity = new Vector3(0, -downSpeed, 0);
            }
    }
    void IncrementSpeed()
    {
        if (playerData.GetScooterLevel() > 1 && runOnce != true)
        {
            incrementSpeedPercentage *= playerData.GetScooterLevel();
            speed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
        }
    }
}
