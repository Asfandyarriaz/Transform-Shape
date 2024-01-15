using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatObstacleBehaviour : MonoBehaviour
{
    [SerializeField] BoatMovement boatMovementScript;
    [SerializeField] float rayCastLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 rayCastOffsetFront;
    [SerializeField] Vector3 rayCastOffsetBack;
    [SerializeField] float YOffset;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            boatMovementScript.allowMove = true;
        }
        else
        {
            StartCoroutine(TurnAllowMoveOff());
        }
        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            TriggerWinState();
        }
    }

    private void Update()
    {
        if (IsOnWaterFront() || IsOnWaterBack())
        {
            boatMovementScript.allowMove = true;
            //transform.position = new Vector3(transform.position.x, transform.position.y + YOffset, transform.position.z);
        }
        else
        {
            boatMovementScript.allowMove = false;
        }
    }
    bool IsOnWaterFront()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetFront);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastLength, groundLayer))
        {
            return true;
        }
        return false;
    }
    bool IsOnWaterBack()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffsetBack);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastLength, groundLayer))
        {
            return true;
        }
        return false;
    }

    private IEnumerator TurnAllowMoveOff()
    {
        boatMovementScript.hoverHeight = 1 ;

        float time = 0;
        float duration = 0.5f;
        float value = 0;
        while(time < duration)
        {
            value = Mathf.Lerp(boatMovementScript.hoverHeight, boatMovementScript.hoverHeight / 2, time/duration);
            boatMovementScript.hoverHeight = value;
            time += Time.deltaTime;
            yield return null;
        }
        boatMovementScript.allowMove = false;
        boatMovementScript.hoverHeight = 1;
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
