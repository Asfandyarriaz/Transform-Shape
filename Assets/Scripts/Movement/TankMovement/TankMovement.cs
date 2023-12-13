using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
   
    [SerializeField] public bool allowMove;
    [SerializeField] float slowDown;
    Rigidbody rb;

    //Flags
    [SerializeField] private bool runOnStart = true;

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
        if (state != GameManager.GameState.Play)
            runOnStart = true;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Movement()
    {
        if (allowMove)
        {
            if(runOnStart)
            {
                rb.velocity = (Vector3.forward * vehicleProperties.speed);
            }
            if (rb.velocity.magnitude <= vehicleProperties.speed)
            {
                rb.AddForce((Time.deltaTime * Vector3.forward) * vehicleProperties.speed, ForceMode.Impulse);
            }
        }
    }
}
