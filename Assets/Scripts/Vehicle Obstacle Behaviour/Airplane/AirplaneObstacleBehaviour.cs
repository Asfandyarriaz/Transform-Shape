using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneObstacleBehaviour : MonoBehaviour
{

    [Header("Raycast Front Setting")]
    [SerializeField] Vector3 rayCastOffsetFront;
    [SerializeField] float rayCastLengthFront;

    AirplaneMovement airplaneMovementScript;

    //Flags
    [SerializeField] private bool runOnce = false;
    private void OnCollisionEnter(Collision collision)
    {
        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            TriggerWinState();
        }
    }
    private void Start()
    {
        airplaneMovementScript = GetComponent<AirplaneMovement>();
    }
    private void Update()
    {
        if (Time.frameCount % 2 == 0)
        {
            if (CheckFront())
                airplaneMovementScript.allowMove = false;
            else
                airplaneMovementScript.allowMove = true;
        }
    }

    bool CheckFront()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetFront);

        Debug.DrawRay(rayCastOrigin, Vector3.forward * rayCastLengthFront, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, rayCastLengthFront))
        {
           // if (hit.collider.CompareTag("Fly"))
            //{
                return true;
            //}
            /*if (hit.collider.CompareTag("Win") && GameManager.Instance.State == GameManager.GameState.Play)
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
            }*/
        }
        return false;
    }

    private void ResetFlags()
    {
        runOnce = true;
    }
    private void OnDisable()
    {
        ResetFlags();
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
