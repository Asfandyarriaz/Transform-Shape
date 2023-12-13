using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatObstacleBehaviour : MonoBehaviour
{
    [SerializeField] BoatMovement boatMovementScript;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            boatMovementScript.allowMove = true;
        }
        else
        {
            boatMovementScript.allowMove = false;
        }
        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        }
    }
}
