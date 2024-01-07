using System.Collections;
using UnityEngine;


public class CarObstacleBehaviour : MonoBehaviour
{
    [SerializeField] CarMovement carMovementScript;

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
        if (collision.gameObject.CompareTag("Water"))
        {
            carMovementScript.allowMove = false;
        }
        else
        {
            carMovementScript.allowMove = true;
        }

        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    bool RaycastFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetFront;

        Debug.DrawRay(origin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(origin, Vector3.forward, out hit, rayCastLengthFront))
        {
            if (hit.collider.CompareTag("Stairs"))
            {
                carMovementScript.StopCar();
                carMovementScript.allowMove = false;
                // if(startCoroutine)
                //StartCoroutine(SlowCar());
                return true;
            }
            if (hit.collider.CompareTag("Not Passable"))
            {
                //if (startCoroutine)
                //StartCoroutine(SlowCar());
                carMovementScript.StopCar();
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
                carMovementScript.StopCar();
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

        Debug.DrawRay(origin, Vector3.down * Mathf.Infinity, Color.green);
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
        {
            /*if (hit.collider.CompareTag("Stairs"))
            {
                carMovementScript.StopCar();
                return true;
            }*/
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
                carMovementScript.StopCar();
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
}
