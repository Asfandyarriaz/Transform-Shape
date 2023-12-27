using System.Runtime.CompilerServices;
using UnityEngine;

public class CarMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] public bool allowMove;

    Rigidbody rb;

    [Header("Speed")]
    [SerializeField] private float speed;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage = 5f;

    //Flags
    private bool runOnce = false;

    //Variables
    CarObstacleBehaviour carObstacleBehaviourScript;
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void OnEnable()
    {
        if(carObstacleBehaviourScript != null)
        carObstacleBehaviourScript.enabled = true;
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

        if(state == GameManager.GameState.Cash)
        {
            carObstacleBehaviourScript.enabled = false;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        allowMove = true;
        carObstacleBehaviourScript = GetComponent<CarObstacleBehaviour>();
    }

    public void Movement()
    {
        if (allowMove)
        {
            rb.velocity = Vector3.forward * vehicleProperties.speed;
        }
        else
        {
            rb.velocity = new Vector3(0, -vehicleProperties.speed, 0);
        }
    }

    //5 % Increment with each level
    void IncrementSpeed()
    {
        
        if (vehicleProperties.currentUpgradeLevel > 1 && runOnce != true)
        {
            speed = vehicleProperties.speed;
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel -1;
            speed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }
}
