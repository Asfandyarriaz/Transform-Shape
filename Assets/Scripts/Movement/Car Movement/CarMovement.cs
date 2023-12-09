using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   public void Movement()
    {
        rb.velocity = Vector3.forward * vehicleProperties.speed;
    }
}
