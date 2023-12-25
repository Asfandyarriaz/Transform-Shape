using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] public GameObject[] tranformObjectsArr;
    [SerializeField] public Transform aiStartingPosition;

    //Variables
    int vehicleIndex;
    IInterfaceMovement movementBehaviorSriptAttachedObject;

    //Flags
    private bool hasCheckOnStateChange;
    private bool updateAllowed;

    #region State Handling
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
        if (state == GameManager.GameState.Play)
        {
            updateAllowed = true;
        }
        else
        {
            updateAllowed = false;
            ResetFlags();
        }

        if (state == GameManager.GameState.SetupGameData || state == GameManager.GameState.Start)
        {
            SetInitialVehicleIfAllInactive();
            SetVehiclePosition();
        }
    }
    #endregion

    #region GameLogic
    /// <summary>
    /// Game Logic
    /// </summary>
    void Update()
    {
        if (updateAllowed)
        {
            if (hasCheckOnStateChange) { CheckVehicleAndGetComponent(); }
            movementBehaviorSriptAttachedObject.Movement();
        }
    }
    //TODO:Check Performance hit this function keeps running in update. Good practice ? 
    private void CheckVehicleAndGetComponent()
    {
        vehicleIndex = CheckActiveVehicle();
        movementBehaviorSriptAttachedObject = tranformObjectsArr[vehicleIndex].gameObject.GetComponent<IInterfaceMovement>();
        hasCheckOnStateChange = false;
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
    public GameObject CheckActiveVehicle(string check)
    {
        for (int i = 0; i < tranformObjectsArr.Length; i++)
        {
            if (tranformObjectsArr[i].gameObject.activeSelf)
            { return tranformObjectsArr[i].gameObject; }
        }
        if (!tranformObjectsArr[0].gameObject.activeSelf) { tranformObjectsArr[0].gameObject.SetActive(true); }

        return tranformObjectsArr[0].gameObject; //Return 0 by default
    }
    private void ResetFlags()
    {
        hasCheckOnStateChange = true;
    }
    void SetVehiclePosition()
    {
        for (int i = 0; i < tranformObjectsArr.Length; i++)
        {
            tranformObjectsArr[i].transform.position = aiStartingPosition.position;
        }
    }
    void SetInitialVehicleIfAllInactive()
    {
        bool isVehicleActive = false;
        for(int i =0; i<tranformObjectsArr.Length;i++)
        {
            if(tranformObjectsArr[i].activeSelf)
            {
                isVehicleActive = true;
                break;
            }
        }
        if (isVehicleActive != true)
        {
            if (!tranformObjectsArr[0].gameObject.activeSelf) { tranformObjectsArr[0].gameObject.SetActive(true); }
        }
    }
    #endregion
}
