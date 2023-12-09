using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Movement()
    {
        characterController.Move(Vector3.forward * vehicleProperties.speed * Time.deltaTime);
    }
}
