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
    // Start is called before the first frame update
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
                characterController.Move(Vector3.up * vehicleProperties.speed * Time.deltaTime);
            }
            else
            {
                characterController.Move(Vector3.forward * vehicleProperties.speed * Time.deltaTime);
            }
        }
        else
        {
            characterController.Move(Vector3.down * vehicleProperties.speed * Time.deltaTime);
        }
        if(!isOnGround && !isClimbable) { characterController.Move(Vector3.down * vehicleProperties.speed * Time.deltaTime); }
    }
}
