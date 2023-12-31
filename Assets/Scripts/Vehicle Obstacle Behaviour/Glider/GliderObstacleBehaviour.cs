using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderObstacleBehaviour : MonoBehaviour
{
    [SerializeField] GliderMovement gliderMovementScript;

    [Header("Raycast Setting Front")]
    [SerializeField] float rayCastLengthFront;
    [SerializeField] Vector3 rayCastOffsetFront;

    [Header("Raycast Setting Down")]
    //[SerializeField] float rayCastLengthDown;
    [SerializeField] Vector3 rayCastOffsetDown;

    [Header("Layer")]
    [SerializeField] LayerMask groundLayer;

    //Varaibles


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Stop Logic
        }
        
        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
        }
    }


    bool RaycastFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetFront;

        Debug.DrawRay(origin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(origin, Vector3.forward, out hit, rayCastLengthFront))
        {
            if (hit.collider.CompareTag("Stairs"))
            {

                return true;
            }
        }
        return false;
    }

    bool RaycastDown()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetDown;

        Debug.DrawRay(origin, Vector3.down * Mathf.Infinity, Color.green);
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Stairs"))
            {
                return true;
            }
        }
        return false;
    }
    
}
