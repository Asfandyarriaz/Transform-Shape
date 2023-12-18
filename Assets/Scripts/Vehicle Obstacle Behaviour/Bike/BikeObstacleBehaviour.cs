using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeObstacleBehaviour : MonoBehaviour
{
    [SerializeField] BikeMovement bikeMovementScript;

    [Header("Raycast Setting Ground")]
    [SerializeField] float rayCastLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 rayCastOffset;

    [Header("Raycast Setting Front")]
    [SerializeField] float rayCastLengthFront;
    [SerializeField] Vector3 rayCastOffsetFront;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            /*collision.gameObject.SetActive(false);
            //Play Audio
            AudioManager.Instance.PlaySFX(AudioManager.Instance.objectBreakSound);*/
        }
        if (collision.gameObject.CompareTag("Ramp"))
        {
            bikeMovementScript.allowMove = false;
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            bikeMovementScript.allowMove = false;
        }
        else
        {
            bikeMovementScript.allowMove = true;
        }

        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        }
    }


    private void Update()
    {
        if (IsGrounded() && !CheckRaycastFront() || CheckForBikeTrail())
        {
            bikeMovementScript.allowMove = true;
        }
        else
        {
            bikeMovementScript.allowMove = false;
        }
        CheckForBikeTrail();

    }
    bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffset);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastLength, groundLayer))
        {
            return true;
        }
        return false;
    }
    bool CheckRaycastFront()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetFront);

        Debug.DrawRay(rayCastOrigin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, rayCastLengthFront, groundLayer))
        {
            return true;
        }
        return false;
    }

    bool CheckForBikeTrail()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffset);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastLength, Color.blue);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastLength))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Biketrail"))
            {
                return true;
            }
        }
        return false;
    }

}
