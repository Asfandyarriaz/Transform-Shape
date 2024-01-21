using System.Collections;
using UnityEngine;

public class TankObstacleBehaviour : MonoBehaviour
{
    [SerializeField] TankMovement tankMovementScript;

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

    [Header("Obstacle Raycast Setting")]
    [SerializeField] Vector3 obstacleRaycastOffsetDown;
    [SerializeField] float obstacleRaycastLength;


    //Flags
    public bool slowCheck;
    public bool startCoroutine = true;
    private bool startCoroutineRotate = true;
    private bool stopWinCounter = false;

    //Varaibles
    private float stopRotateSeconds = 2f;

    //Flags


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            hit.collider.gameObject.SetActive(false);
        }
        //Win Check
        if (hit.gameObject.CompareTag("Win"))
        {
            TriggerWinState();
        }
    }
    private void Update()
    {
        if (Time.frameCount % 2 == 0)
        {
            if (RaycastFront() || RaycastDownCheckForStairs())
            {
                //Debug.Log("Allow Move False In Condition");
                tankMovementScript.allowMove = false;
            }
            else if (!RaycastFront() && !RaycastDownCheckForStairs())
            {
                //Debug.Log("Allow Move True In Condition");
                tankMovementScript.allowMove = true;
            }

            RaycastForSlowCheck();         
        }
        RaycastObstacleCheck();
        RaycastGravity();
    }
    #region Raycast Check For Stairs
    //Cast a ray to front of character to check for any stairs or Climbable surface
    bool RaycastFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetFront;

        //Debug.DrawRay(origin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(origin, Vector3.forward, out hit, rayCastLengthFront))
        {
            if (hit.collider.CompareTag("Stairs"))
            {
                tankMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("Not Passable"))
            {
                tankMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("PassThrough"))
            {
                if (startCoroutineRotate)
                    StartCoroutine(TurnOffRotate());
            }
            if (hit.collider.CompareTag("Climbable"))
            {
                tankMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("Fly"))
            {
                tankMovementScript.allowMove = false;
                return true;
            }
        }
        return false;
    }

    bool RaycastForSlowCheck()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetDown;

        //Debug.DrawRay(origin, Vector3.down * (rayCastDownLength + 2), Color.black);
        if (Physics.Raycast(origin, Vector3.down, out hit, rayCastDownLength + 2))
        {
            if (hit.collider.CompareTag("Water"))
            {
                if (slowCheck)
                {
                    slowCheck = false;
                    StartCoroutine(tankMovementScript.SlowSpeedInDifferentTerrain());
                }
            }
            else if (hit.collider.CompareTag("Biketrail"))
            {
                if (slowCheck)
                {
                    slowCheck = false;
                    StartCoroutine(tankMovementScript.SlowSpeedInDifferentTerrain());
                }
            }
            else
            {
                if (slowCheck == false)
                {
                    slowCheck = true;
                    StartCoroutine(tankMovementScript.ResetSpeed());
                }
            }

        }

        return false;
    }
    bool RaycastDownCheckForStairs()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetDown;

        //Debug.DrawRay(origin, Vector3.down * rayCastDownLength, Color.green);
        if (Physics.Raycast(origin, Vector3.down, out hit, rayCastDownLength))
        {
            //Debug.Log("Raycast Down For Stairs : " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Stairs"))
            {
                return true;
            }
        }

        return false;
    }

    //Apply a raycast with a small length to check for grounded if detected reset gravity
    void RaycastGravity()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + gravityRaycastOffsetDown;
        //Debug.DrawRay(origin, Vector3.down * gravityRaycastLength, Color.white);
        if (Physics.Raycast(origin, Vector3.down, out hit, gravityRaycastLength, groundLayer))
        {
            tankMovementScript.velocity.y = 0;
        }
    }

    void RaycastObstacleCheck()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + obstacleRaycastOffsetDown;
        Debug.DrawRay(origin, Vector3.forward * obstacleRaycastLength, Color.white);
        if (Physics.Raycast(origin, Vector3.forward, out hit, obstacleRaycastLength, groundLayer))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                //TODO: Play Destruction Particles
                hit.collider.gameObject.SetActive(false);
            }
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
        tankMovementScript.allowRotate = false;
        yield return new WaitForSeconds(stopRotateSeconds);
        tankMovementScript.allowRotate = true;
        startCoroutineRotate = true;
    }
}

#endregion