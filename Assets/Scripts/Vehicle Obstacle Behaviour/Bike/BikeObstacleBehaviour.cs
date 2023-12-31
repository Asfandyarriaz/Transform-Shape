using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeObstacleBehaviour : MonoBehaviour
{
    [SerializeField] BikeMovement bikeMovementScript;

    [Header("Raycast Setting Front")]
    [SerializeField] float rayCastLengthFront;
    [SerializeField] Vector3 rayCastOffsetFront;

    [Header("Raycast Setting Down")]
    //[SerializeField] float rayCastLengthDown;
    [SerializeField] Vector3 rayCastOffsetDown;

    [Header("Layer")]
    [SerializeField] LayerMask groundLayer;

    //Varaibles
    Rigidbody rb;

    //Flags
    private bool slowCheck;
    private bool startCoroutine = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Stop Car Logic
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
        if (RaycastFront() || RaycastDown())
        {
            bikeMovementScript.allowMove = false;
        }

        if (!RaycastFront() && !RaycastDown())
        {
            bikeMovementScript.allowMove = true;
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
                if (startCoroutine)
                    StartCoroutine(SlowCar());
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
            if (hit.collider.CompareTag("Water"))
            {
                if (slowCheck)
                {
                    slowCheck = false;
                    StartCoroutine(bikeMovementScript.SlowSpeedInWater());
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
}
