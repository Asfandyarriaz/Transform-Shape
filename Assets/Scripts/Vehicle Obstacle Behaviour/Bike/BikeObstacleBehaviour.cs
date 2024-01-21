using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class BikeObstacleBehaviour : MonoBehaviour
{
    BikeMovement bikeMovementScript;
    [Header("Raycast Setting Front")]
    [SerializeField] float rayCastLengthFront;
    [SerializeField] Vector3 rayCastOffsetFront;

    [Header("Raycast Setting Down")]
    //[SerializeField] float rayCastLengthDown;
    [SerializeField] Vector3 rayCastOffsetDown;
    [SerializeField] float rayCastDownLength;

    [Header("Gravity Raycast Setting")]
    [SerializeField] Vector3 gravityRaycastOffsetDown;
    [SerializeField] float gravityRaycastLength;
    [SerializeField] LayerMask groundLayer;


    //Flags
    public bool slowCheck;
    public bool startCoroutine = true;
    private bool startCoroutineRotate = true;
    private bool stopWinCounter = false;

    //Varaibles
    private float stopRotateSeconds = 2f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Win Check
        if (hit.gameObject.CompareTag("Win"))
        {
            TriggerWinState();
        }
    }
    private void Start()
    {
        bikeMovementScript = GetComponent<BikeMovement>();
        slowCheck = true;
    }
    private void Update()
    {
        if (Time.frameCount % 2 == 0)
        {
            if ((RaycastFront() || RaycastDownCheckForStairs()))
            {
                bikeMovementScript.allowMove = false;
            }
            else if (!RaycastFront() && !RaycastDownCheckForStairs())
            {
                bikeMovementScript.allowMove = true;
            }

            RaycastForSlowCheck();
        }
        RaycastGravity();
    }
    #region Raycast Check For Stairs
    //Cast a ray to front of character to check for any stairs or Climbable surface
    bool RaycastFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetFront;

        Debug.DrawRay(origin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(origin, Vector3.forward, out hit, rayCastLengthFront))
        {
            if (hit.collider.CompareTag("Stairs"))
            {
                bikeMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("Not Passable"))
            {
                bikeMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("PassThrough"))
            {
                if (startCoroutineRotate)
                    StartCoroutine(TurnOffRotate());
            }
            if (hit.collider.CompareTag("Climbable"))
            {
                bikeMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("Fly"))
            {
                bikeMovementScript.allowMove = false;
                return true;
            }
        }
        return false;
    }

    bool RaycastForSlowCheck()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetDown;

        Debug.DrawRay(origin, Vector3.down * (rayCastDownLength + 2), Color.black);
        if (Physics.Raycast(origin, Vector3.down, out hit, rayCastDownLength + 2))
        {
            if (hit.collider.CompareTag("Water"))
            {

                if (slowCheck)
                {
                    slowCheck = false;
                    StartCoroutine(bikeMovementScript.SlowSpeedInDifferentTerrain());
                }
            }           
            else
            {
                if (slowCheck == false)
                {
                    slowCheck = true;
                    StartCoroutine(bikeMovementScript.ResetSpeed());
                }
            }
        }

        return false;
    }
    bool RaycastDownCheckForStairs()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetDown;

        Debug.DrawRay(origin, Vector3.down * rayCastDownLength, Color.green);
        if (Physics.Raycast(origin, Vector3.down, out hit, rayCastDownLength))
        {

            if (hit.collider.CompareTag("Stairs"))
            {
                return true;
            }
        }
        return false;
    }
    void RaycastGravity()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + gravityRaycastOffsetDown;
        Debug.DrawRay(origin, Vector3.down * gravityRaycastLength, Color.white);
        if (Physics.Raycast(origin, Vector3.down, out hit, gravityRaycastLength, groundLayer))
        {
            bikeMovementScript.velocity.y = 0;
        }
    }

    void TriggerWinState()
    {
        if (transform.parent.name.Equals("TransformList"))
        {
            GameManager.Instance.winPosition++;
            GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
        }
        else
        {
            if (stopWinCounter != true) { GameManager.Instance.winPosition++; }
            stopWinCounter = true;
        }
    }
    IEnumerator TurnOffRotate()
    {
        startCoroutineRotate = false;
        bikeMovementScript.allowRotate = false;
        yield return new WaitForSeconds(stopRotateSeconds);
        bikeMovementScript.allowRotate = true;
        startCoroutineRotate = true;
    }
}

#endregion