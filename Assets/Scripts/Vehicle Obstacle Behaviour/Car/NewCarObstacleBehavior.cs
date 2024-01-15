using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class NewCarObstacleBehavior : MonoBehaviour
{
    [SerializeField] CarMovementController carMovementScript;

    [Header("Raycast Setting Front")]
    [SerializeField] float rayCastLengthFront;
    [SerializeField] Vector3 rayCastOffsetFront;

    [Header("Raycast Setting Down")]
    //[SerializeField] float rayCastLengthDown;
    [SerializeField] Vector3 rayCastOffsetDown;
    [SerializeField] float rayCastDownLength;


    //Flags
    public bool slowCheck;
    public bool startCoroutine = true;
    private bool startCoroutineRotate = true;

    //Varaibles
    private float stopRotateSeconds = 2f;

    //Flags


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            //Stop Car Logic
        }
        if (hit.gameObject.CompareTag("Not Passable"))
        {
            //Stop Car Logic
        }
        if (hit.gameObject.CompareTag("Water"))
        {
            carMovementScript.allowMove = false;
        }
        else
        {
            carMovementScript.allowMove = true;
        }

        //Win Check
        if (hit.gameObject.CompareTag("Win"))
        {
            TriggerWinState();
        }
    }
    private void Update()
    {
        if (RaycastFront() || RaycastDown() || RaycastDownCheckForStairs())
        {
            carMovementScript.allowMove = false;
        }
        else if (!RaycastFront() && !RaycastDown())
        {
            carMovementScript.allowMove = true;
        }

        RaycastDownCheckForStairs();
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
                carMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("Not Passable"))
            {
                carMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("PassThrough"))
            {
                if (startCoroutineRotate)
                    StartCoroutine(TurnOffRotate());
            }
            if (hit.collider.CompareTag("Climbable"))
            {
                carMovementScript.allowMove = false;
                return true;
            }
        }
        return false;
    }

    bool RaycastDown()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetDown;

        Debug.DrawRay(origin, Vector3.down * rayCastDownLength, Color.black);
        if (Physics.Raycast(origin, Vector3.down, out hit, rayCastDownLength))
        {
            if (hit.collider.CompareTag("Water"))
            {
                if (slowCheck)
                {
                    StartCoroutine(carMovementScript.SlowSpeedInWater());
                    slowCheck = false;
                }
            }
            else
            {
                if (slowCheck == false)
                {
                    StartCoroutine(carMovementScript.ResetSpeed());
                    slowCheck = true;
                }
            }
            carMovementScript.velocity.y = 0;
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

    void TriggerWinState()
    {
        if (transform.parent.name.Equals("TransformList"))
        {
            GameManager.Instance.winPosition++;
            GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
        }
        else
        {
            GameManager.Instance.winPosition++;
        }
    }
    IEnumerator TurnOffRotate()
    {
        startCoroutineRotate = false;
        carMovementScript.allowRotate = false;
        yield return new WaitForSeconds(stopRotateSeconds);
        carMovementScript.allowRotate = true;
        startCoroutineRotate = true;
    }
}

#endregion