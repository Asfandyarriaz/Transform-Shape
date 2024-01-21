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
    private float stopRotateSeconds = 5f;
    //Flags
    private bool slowCheck;
    private bool startCoroutine = true;
    private bool startCoroutineRotate = true;


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Jump"))
        {
            Debug.Log("Jump Collider Triggered");
            gliderMovementScript.allowJump = true;
            StartCoroutine(gliderMovementScript.Jump());
        }

        //Win Check
        if (hit.gameObject.CompareTag("Win"))
        {
            TriggerWinState();
        }
    }

    private void Update()
    {
        RaycastFront();
    }

    void RaycastFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + rayCastOffsetFront;

        Debug.DrawRay(origin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(origin, Vector3.forward, out hit, rayCastLengthFront))
        {        
            if(hit.collider.CompareTag("Jump"))
            {
                Debug.Log("Jump Collider Triggered");
                gliderMovementScript.allowJump = true;
                StartCoroutine(gliderMovementScript.Jump());
            }
        }
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
}
