using System.Runtime.CompilerServices;
using UnityEngine;

public class CarMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] public bool allowMove;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    Rigidbody rb;

    //Flags
    [SerializeField] private float smoothTime;

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

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        allowMove = true;
    }

    public void Movement()
    {
        bool setConstantSpeed = false;
        if (allowMove)
        {
            float currentSpeed = rb.velocity.magnitude;
            if (currentSpeed < vehicleProperties.speed && !setConstantSpeed)
            {
                // Gradually increase the force to accelerate
                float forceMagnitude = acceleration * Time.fixedDeltaTime;
                rb.AddForce(transform.forward * forceMagnitude, ForceMode.VelocityChange);
            }
            if (currentSpeed >= vehicleProperties.speed || setConstantSpeed)
            {
                setConstantSpeed = true;
                rb.velocity = (Vector3.forward * rb.velocity.magnitude);
            }
            if (rb.velocity.magnitude <= 0.5f)
                setConstantSpeed = false;
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}
