using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneObstacleBehaviour : MonoBehaviour
{
    BezierFollow bezierFollowScipt;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Fly"))
        {
            bezierFollowScipt.startBezierCurve = true;
        }
    }
}
