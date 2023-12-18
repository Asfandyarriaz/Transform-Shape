using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] public VehicleProperties vehicleProperties;
    [SerializeField] public bool allowMove;
    private float downSpeed;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        downSpeed = vehicleProperties.speed;
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
}
