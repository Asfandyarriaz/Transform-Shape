using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] public bool allowMove;
    Rigidbody rb;

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
    }
}
