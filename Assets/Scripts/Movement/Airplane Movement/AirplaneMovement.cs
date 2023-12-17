using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] BezierFollow bezierFollowScript;

    private void Start()
    {
        bezierFollowScript.speedModifier = vehicleProperties.speed;
    }

    public void Movement()
    {

    }
}
