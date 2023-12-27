using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] public GameObject[] tranformObjectsArr;
    [SerializeField] public Transform startingPosition;

    //Variables
    int vehicleIndex;
    IInterfaceMovement movementBehaviorSriptAttachedObject;

    [Header("Flags")]
    //Flags
    [SerializeField] private bool hasCheckOnStateChange;
    [SerializeField] private bool updateAllowed;

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
        if (state == GameManager.GameState.Play)
        {
            updateAllowed = true;
            
        }
        if (state == GameManager.GameState.Play)
        {
            updateAllowed = true;
            ResetFlags();

        }
        else
        {
            updateAllowed = false;
            ResetFlags();
        }

        if (state == GameManager.GameState.Start)
        {
            SetVehiclePosition();
        }
    }
    #endregion

    #region GameLogic
    /// <summary>
    /// Game Logic
    /// </summary>
    void FixedUpdate()
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
            tranformObjectsArr[i].transform.position = startingPosition.position;
        }
    }
    public void OnClickChangeState()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }
    #endregion
}
