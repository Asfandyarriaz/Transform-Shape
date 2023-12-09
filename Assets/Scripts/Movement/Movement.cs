using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject[] tranformObjectsArr;
    [SerializeField] Transform startingPosition;
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

        if(state == GameManager.GameState.Start)
            SetVehiclePosition();
    }
    #endregion

    private void Start()
    {
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
    //TODO:Check Performance hit this function keeps running in update. Good practice ? 
    private void CheckVehicleAndGetComponent()
    {
         vehicleIndex = CheckActiveVehicle();
         movementBehaviorSriptAttachedObject = tranformObjectsArr[vehicleIndex].gameObject.GetComponent<IInterfaceMovement>();
    }
    int CheckActiveVehicle()
    {
        for (int i = 0; i < tranformObjectsArr.Length; i++)
        {
            if (tranformObjectsArr[i].gameObject.activeSelf)
            { return i; }
        }
        if (!tranformObjectsArr[0].gameObject.activeSelf) { tranformObjectsArr[0].gameObject.SetActive(true); }

        return 0; //Return 0 by default
    }
    private void ResetFlags()
    {
        hasCheckOnStateChange = true;
    }
    void SetVehiclePosition()
    {
        for(int i = 0;i <  tranformObjectsArr.Length;i++)
        {
            tranformObjectsArr[i].transform.position = startingPosition.position;
        }
    }
    public void OnClickChangeState()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }
    #endregion
}
