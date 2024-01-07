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
            //tankMovementScript.ForceBack(collision.transform.position);
            collision.gameObject.SetActive(false);
            //Play Audio
            AudioManager.Instance.PlaySFX(AudioManager.Instance.objectBreakSound);
        }

        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
        }
    }
    private void Update()
    {
        if(RaycastFront() || RaycastDown())
        {
            tankMovementScript.allowMove = false;
        }

        if(!RaycastFront() && !RaycastDown())
        {
            tankMovementScript.allowMove = true;
        }     
    }

    bool RaycastFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetFront;

        Debug.DrawRay(origin,Vector3.forward * rayCastLengthFront,Color.red);
        if(Physics.Raycast(origin,Vector3.forward, out hit,rayCastLengthFront)) 
        {
            if(hit.collider.CompareTag("Stairs"))
            {
                return true;
            }
            if (hit.collider.CompareTag("Not Passable"))
            {
                //if (startCoroutine)
                //StartCoroutine(SlowCar());
                tankMovementScript.StopCar();
                tankMovementScript.allowMove = false;
                return true;
            }
            if (hit.collider.CompareTag("PassThrough"))
            {
                if (startCoroutineRotate)
                    StartCoroutine(TurnOffRotate());
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
            if (hit.collider.CompareTag("Water"))
            {
                if (slowCheck)
                {
                    slowCheck = false;
                    StartCoroutine(tankMovementScript.SlowSpeedInWater());
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
    IEnumerator TurnOffRotate()
    {
        startCoroutineRotate = false;
        tankMovementScript.allowRotate = false;
        yield return new WaitForSeconds(stopRotateSeconds);
        tankMovementScript.allowRotate = true;
        startCoroutineRotate = true;
    }
}
