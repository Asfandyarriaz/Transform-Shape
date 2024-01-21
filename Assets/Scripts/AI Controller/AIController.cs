using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] public GameObject[] tranformObjectsArr;
    [SerializeField] public Transform aiStartingPosition;

    [Header("Difficulty Settings")]
    public Difficulty difficulty;

    //Variables
    int vehicleIndex;
    IInterfaceMovement movementBehaviorSriptAttachedObject;

    //Flags
    public bool hasVehicleChanged;
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

        if (/*state == GameManager.GameState.SetupGameData || state == GameManager.GameState.Start || */state == GameManager.GameState.Play)
        {
            SetVehiclePosition();
            SetInitialVehicleIfAllInactive();
            //StartCoroutine(SetVehiclePosition());
            //StartCoroutine(RunFunctionsInSequence());
            
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
    public int CheckActiveVehicle()
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
        hasVehicleChanged = true;
    }
    bool SetVehiclePosition()
    {
        for (int i = 0; i < tranformObjectsArr.Length; i++)
        {
            tranformObjectsArr[i].transform.position = aiStartingPosition.position;
           // Debug.Log("Transform Obj" + i + " Position : " + tranformObjectsArr[i].transform.position + " Name : " + transform.parent.name + " Level Name : " + transform.parent.parent.parent.name);
            //Debug.Log("1st Func Time : " + Time.time);
        }
        
        return true;
    }
    /*IEnumerator SetVehiclePosition()
    {
        while(!transform.parent.gameObject.activeSelf) { yield return null; }

        int i = 0;
        do
        {
            //for (int i = 0; i < tranformObjectsArr.Length; i++)
            tranformObjectsArr[i].transform.position = aiStartingPosition.position;
            Debug.Log("Transform Obj" + i + " Position : " + tranformObjectsArr[i].transform.position + " Name : " + transform.parent.name);
            i++;
        } while (i < tranformObjectsArr.Length);

        
    }*/
    void SetInitialVehicleIfAllInactive()
    {
        bool isVehicleActive = false;
        for (int i = 0; i < tranformObjectsArr.Length; i++)
        {
            if (tranformObjectsArr[i].activeSelf)
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

    private IEnumerator RunFunctionsInSequence()
    {
        // Execute the first function
        while(!SetVehiclePosition())
        {
            
            yield return null;
        }

        //Debug.Log("2nd Func Time : " + Time.time);
        // Now, execute the second function
        SetInitialVehicleIfAllInactive();
    }
    #endregion


    //Generate success probabilty based on dificulty
    #region Difficulty
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    // Method to generate a bool based on the specified difficulty setting
    public bool GenerateProbability(Difficulty difficulty)
    {
        float successRate = GetSuccessRate(difficulty);

        // Generate a random float between 0 and 1
        float randomValue = Random.Range(0f, 1f);

        // Check if the random value is less than the success rate
        bool isSuccess = randomValue < successRate;

        return isSuccess;
    }

    private float GetSuccessRate(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 0.25f; // 25% success rate
            case Difficulty.Medium:
                return 0.35f; // 35% success rate
            case Difficulty.Hard:
                return 0.5f; // 50% success rate
            default:
                Debug.LogError("Invalid difficulty setting");
                return 0f;
        }
    }
    #endregion
}
