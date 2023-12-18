using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] BezierFollow bezierFollowScript;

    [Header("Set above the ground")]
    [SerializeField] float yOffset;

    private void Start()
    {
        bezierFollowScript.speedModifier = vehicleProperties.speed;
    }

    public void Movement()
    {
        if (!bezierFollowScript.bezierRunning)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                Vector3 newPosition = hit.point + new Vector3(0, yOffset, 0);
                transform.position = newPosition;
            }
        }
    }
}
