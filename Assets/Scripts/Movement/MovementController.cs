using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] public GameObject[] tranformObjectsArr;
    [SerializeField] public Transform startingPosition;
    [SerializeField] LevelManager levelManagerScript;

    //Variables
    int vehicleIndex;
    IInterfaceMovement movementBehaviorSriptAttachedObject;

    [Header("Flags")]
    //Flags
    [SerializeField] public bool hasVehicleChanged;
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
            ResetFlags();

        }
        else
        {
            updateAllowed = false;
            ResetFlags();
        }

        if (state == GameManager.GameState.Start)
        {
            UserStartPoint();
            SetVehiclePosition();
            SetDefaultVehicle();
        }
    }
    #endregion

    #region GameLogic
    /// <summary>
    /// Game Logic
    /// </summary>
    /// 
    private void Start()
    {
        UserStartPoint();
        SetVehiclePosition();
        SetDefaultVehicle();
    }
    void FixedUpdate()
    {
        if (updateAllowed)
        {
            if (hasVehicleChanged) { CheckVehicleAndGetComponent(); }
                movementBehaviorSriptAttachedObject.Movement();
        }
    }
    //TODO:Check Performance hit this function keeps running in update. Good practice ? 
    private void CheckVehicleAndGetComponent()
    {
        vehicleIndex = CheckActiveVehicle();
        movementBehaviorSriptAttachedObject = tranformObjectsArr[vehicleIndex].gameObject.GetComponent<IInterfaceMovement>();
        hasVehicleChanged = false;
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

    void SetDefaultVehicle()
    {
        for (int i = 0; i < tranformObjectsArr.Length; i++)
        {
            if (tranformObjectsArr[i].gameObject.activeSelf)
            { tranformObjectsArr[i].gameObject.SetActive(false); }
        }
        //Set Default Vehicle
        if (!tranformObjectsArr[0].gameObject.activeSelf) { tranformObjectsArr[0].gameObject.SetActive(true); }
    }
    private void ResetFlags()
    {
        hasVehicleChanged = true;
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

    void UserStartPoint()
    {
        GameObject currentLevel = levelManagerScript.GetCurrentActiveLevels(PlayerDataController.Instance.playerData.currentLevel);

        for(int i = 0; i < currentLevel.transform.childCount; i++)
        {
            if(currentLevel.transform.GetChild(i).gameObject.name.Equals("User Start Point"))
            {
                startingPosition = currentLevel.transform.GetChild(i).transform;
            }
        }
    }
    #endregion
}
