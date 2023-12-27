using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneObstacleBehaviour : MonoBehaviour
{
    [SerializeField] BezierFollow bezierFollowScipt;

    [Header("Raycast Front Setting")]
    [SerializeField] Vector3 rayCastOffsetFront;
    [SerializeField] float rayCastLengthFront;

    //Flags
    [SerializeField] private bool runOnce = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
        }
    }

    private void Update()
    {
        if(CheckFront() && runOnce != true)
        {
            runOnce = true;
            bezierFollowScipt.startBezierCurve = true;
        }
    }

    bool CheckFront()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetFront);

        Debug.DrawRay(rayCastOrigin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, rayCastLengthFront))
        {
            if(hit.collider.CompareTag("Fly"))
            {
                return true;
            }
        }
        return false;
    }
}
