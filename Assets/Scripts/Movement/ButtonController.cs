using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    MovementController movementControllerScript;
    private void Start()
    {
        movementControllerScript = GetComponent<MovementController>();
    }
    public void OnClickTransformCharacterWalk()
    {
        ChangeToObject("Character Walk", DisableCurrentActive()); //Set name same as object under TransformList object in hierarchy
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
    }

    public void OnClickTransformCar()
    {
        ChangeToObject("Car", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
    }

    Transform DisableCurrentActive()
    {
        for(int i = 0; i < movementControllerScript.tranformObjectsArr.Length; i++) 
        {
            if (movementControllerScript.tranformObjectsArr[i].activeSelf)
            {
                movementControllerScript.tranformObjectsArr[i].SetActive(false);
                return movementControllerScript.tranformObjectsArr[i].transform;
            }
        }
        return null;
    }

    //Change to vehicle and move to previously selected vehicle 
    void ChangeToObject(string transformToName, Transform currentPos)
    {
        for (int i = 0; i < movementControllerScript.tranformObjectsArr.Length; i++)
        {
            if (movementControllerScript.tranformObjectsArr[i].name == transformToName)
            {
                movementControllerScript.tranformObjectsArr[i].transform.position = currentPos.position;
                movementControllerScript.tranformObjectsArr[i].SetActive(true);
                break;
            }
        }
    }
}
