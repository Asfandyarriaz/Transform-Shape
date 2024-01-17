using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterObstacleBehaviour : MonoBehaviour
{
    [SerializeField] CharacterMovement characterMovementScript;
    [SerializeField] float rayCastFrontLength;
    [SerializeField] float rayCastBottomRightLength;
    [SerializeField] float rayCastGroundLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 rayCastOffsetFront;
    [SerializeField] Vector3 rayCastOffsetGround;
    [SerializeField] float rayCastAngle;
    [SerializeField] Vector3 angleOffset;

    [Header("Raycast Check For Climbable Surface")]
    [SerializeField] float rayCastLengthClimbable;
    [SerializeField] Vector3 rayCastOffsetClimbableTop;
    [SerializeField] Vector3 rayCastOffsetClimbableBottom;

    [Header("Gravity Raycast Setting")]
    [SerializeField] Vector3 gravityRaycastOffsetDown;
    [SerializeField] float gravityRaycastLength;

    //Flags
    private bool slowCheck;
    private bool stopWinCounter = false;
    

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            
        }

        //Win Check
        if (hit.gameObject.CompareTag("Win"))
        {
            TriggerWinState();
        }
    }
    private void Update()
    {
        if (CheckFrontClimbableTop() || CheckFrontClimbableBottom())
        {
            characterMovementScript.isClimbable = true;
            characterMovementScript.allowMove = true;
        }
        else
        {
            StartCoroutine(TurnClimbingOff());
        }
        RaycastForSlowCheck();
        RaycastGravity();
    }
    #region Raycast Check For Stairs
    //Cast a ray to front of character to check for any stairs or Climbable surface
    bool CheckFront()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetFront);

        Debug.DrawRay(rayCastOrigin, Vector3.forward * rayCastFrontLength, Color.blue);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, rayCastFrontLength, groundLayer))
        {
            return true;
        }
        return false;
    }
    //Cast a ray to front of character at a slanted angle to check for any stairs
    bool CheckBottomRight()
    {
        RaycastHit hit;
        Vector3 rayCastDirection = Quaternion.Euler(rayCastAngle, 0, 0) * transform.forward;

        Debug.DrawRay((transform.position + angleOffset), rayCastDirection * rayCastBottomRightLength, Color.red);
        if (Physics.Raycast((transform.position + angleOffset), rayCastDirection, out hit, rayCastBottomRightLength, groundLayer))
        {
            return true;
        }
        return false;
    }
    //Cast a ray on the ground to check for if is on ground
    void RaycastForSlowCheck()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetGround);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastGroundLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastGroundLength, groundLayer))
        {
            if (hit.collider.CompareTag("Water"))
            {
                if (slowCheck)
                {
                    slowCheck = false;
                    StartCoroutine(characterMovementScript.SlowSpeedInDifferentTerrain());
                }
                
            }
            else if (hit.collider.CompareTag("Biketrail"))
            {
                if (slowCheck)
                {
                    slowCheck = false;
                    StartCoroutine(characterMovementScript.SlowSpeedInDifferentTerrain());
                }
            }
            else
            {
                if (slowCheck == false)
                {
                    slowCheck = true;
                    StartCoroutine(characterMovementScript.ResetSpeed());
                }
            }
        }
    }
    #endregion

    #region Raycast Check For Climbable Surface 
    //Cast a ray to front of character to check for any stairs or Climbable surface
    bool CheckFrontClimbableTop()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetClimbableTop);

        Debug.DrawRay(rayCastOrigin, Vector3.forward * rayCastLengthClimbable, Color.black);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, rayCastLengthClimbable, groundLayer))
        {
            if (hit.collider.CompareTag("Climbable"))
                return true;
        }
        return false;
    }
    bool CheckFrontClimbableBottom()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetClimbableBottom);

        Debug.DrawRay(rayCastOrigin, Vector3.forward * rayCastLengthClimbable, Color.black);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, rayCastLengthClimbable, groundLayer))
        {
            if (hit.collider.CompareTag("Climbable"))
                return true;
        }
        return false;
    }
    #endregion

    private IEnumerator TurnClimbingOff()
    {
        yield return new WaitForSeconds(0.25f);
        characterMovementScript.isClimbable = false;
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
    //Apply a raycast with a small length to check for grounded if detected reset gravity
    void RaycastGravity()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + gravityRaycastOffsetDown;
        Debug.DrawRay(origin, Vector3.down * gravityRaycastLength, Color.white);
        if (Physics.Raycast(origin, Vector3.down, out hit, gravityRaycastLength, groundLayer))
        {
            characterMovementScript.velocity.y = 0;
        }
    }
}
