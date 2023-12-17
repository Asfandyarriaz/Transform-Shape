using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatObstacleBehaviour : MonoBehaviour
{
    [SerializeField] BoatMovement boatMovementScript;
    [SerializeField] float rayCastLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 rayCastOffsetFront;
    [SerializeField] Vector3 rayCastOffsetBack;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            boatMovementScript.allowMove = true;
        }
        else
        {
            boatMovementScript.allowMove = false;
        }
        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        }
    }

    private void Update()
    {
        if (IsOnWaterFront() || IsOnWaterBack())
        {
            boatMovementScript.allowMove = true;
        }
        else
        {
            boatMovementScript.allowMove = false;
        }
    }
    bool IsOnWaterFront()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetFront);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastLength, groundLayer))
        {
            return true;
        }
        return false;
    }
    bool IsOnWaterBack()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetBack);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastLength, groundLayer))
        {
            return true;
        }
        return false;
    }
}
