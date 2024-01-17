using System.Collections;
using UnityEngine;


public class CarObstacleBehaviour : MonoBehaviour
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
    private bool stopWinCounter = false;

    //Varaibles
    private float stopRotateSeconds = 2f;
    Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Stop Car Logic
        }
        if (collision.gameObject.CompareTag("Not Passable"))
        {
            //Stop Car Logic
        }
        //if (collision.gameObject.CompareTag("Water"))
        //{
        //    carMovementScript.allowMove = false;
        //}
        //else
        //{
        //    carMovementScript.allowMove = true;
        //}

        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
                TriggerWinState();           
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (RaycastFront() || RaycastDownCheckForStairs())
        {
            Debug.Log("Allow Move False In Condition");
            carMovementScript.allowMove = false;
        }
        else if (!RaycastFront() && !RaycastDownCheckForStairs())
        {
            Debug.Log("Allow Move True In Condition");
            carMovementScript.allowMove = true;
        }

        RaycastForSlowCheck();
    }

    bool RaycastFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetFront;

        Debug.DrawRay(origin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(origin, Vector3.forward, out hit, rayCastLengthFront))
        {
            Debug.Log("Raycast Front : " + hit.collider.gameObject.name);
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

    bool RaycastForSlowCheck()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetDown;

        Debug.DrawRay(origin, Vector3.down * Mathf.Infinity, Color.green);
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit Collider : " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Water"))
            {
                if (slowCheck)
                {
                    StartCoroutine(carMovementScript.SlowSpeedInDifferentTerrain());
                }
            }
            else
            {
                if (slowCheck == false)
                {
                    StartCoroutine(carMovementScript.ResetSpeed());
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
            Debug.Log("Raycast Down For Stairs : " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Stairs"))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator SlowCar()
    {
        startCoroutine = false;
        float time = 0;
        float duration = 1f;
        while (time < duration)
        {
            rb.velocity = new Vector3(0, 0, Mathf.Lerp(rb.velocity.z, 0, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        startCoroutine = true;
    }

    IEnumerator TurnOffRotate()
    {
        startCoroutineRotate = false;
        carMovementScript.allowRotate = false;
        yield return new WaitForSeconds(stopRotateSeconds);
        carMovementScript.allowRotate = true;
        startCoroutineRotate = true;
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
}
