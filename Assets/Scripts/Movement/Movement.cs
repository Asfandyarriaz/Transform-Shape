using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject[] tranformObjectsArr;
    CharacterController characterController;
    IEnumerator playCoroutine;

    //Variables
    int vehicleIndex;
    IInterfaceMovement movementBehaviorSriptAttachedObject;

    //Flags
    private bool hasCheckOnStateChange;

    #region State Handling
    /// <summary>
    /// State Handling
    /// </summary>
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Play)
        {
            StartCoroutine(playCoroutine);
        }
        else
        {
            StopCoroutine(playCoroutine);
            ResetFlags();
        }
    }
    #endregion

    private void Start()
    {
        //characterController = tranformObjectsArr.GetComponent<CharacterController>();
        playCoroutine = Play();
    }
    #region GameLogic
    /// <summary>
    /// Game Logic
    /// </summary>
    IEnumerator Play()
    {
        while(true)
        {
            if (hasCheckOnStateChange){ CheckVehicleAndGetComponent(); }
            movementBehaviorSriptAttachedObject.Movement();
            yield return null;
        }
    }
    int CheckActiveVehicle()
    {
        for (int i = 0; i < tranformObjectsArr.Length; i++)
        {
            if (tranformObjectsArr[i].gameObject.activeSelf)
            { return i; }    
        }
        return 0; //Return 0 by default
    }

    //TODO:Check Performance hit this function keeps running in update. Good practice ? 
    private void CheckVehicleAndGetComponent()
    {
         vehicleIndex = CheckActiveVehicle();
         movementBehaviorSriptAttachedObject = tranformObjectsArr[vehicleIndex].gameObject.GetComponent<IInterfaceMovement>();
    }

    private void ResetFlags()
    {
        hasCheckOnStateChange = true;
    }
    public void OnClickChangeState()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }
    #endregion
}
