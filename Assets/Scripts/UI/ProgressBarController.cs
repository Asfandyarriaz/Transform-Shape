using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
/// <summary>
/// Get AI Transform list from current active level
/// Get Finish Line from
/// </summary>
public class ProgressBarController : MonoBehaviour
{
    /// <summary>
    /// Logic Summary : Find current active level -> Go inside the level and find the obj AI 
    /// -> Inside AI get individual components of currently active ai -> Get transform list from them
    /// -> Use the the transform list to find current active vehicle -> use Vector3.Distance to find distance between active vehicle
    /// and finish line -> Use percentage logic to check how much distance is covered 
    /// -> apply value obtained into slider to get current position in the races
    /// </summary>
    [Header("User Transform List")]
    [SerializeField] private GameObject[] transformListArray;
    private Transform player; // Player's transform

    [Header("Level Manager")]
    [SerializeField] LevelManager levelManagerScript;
    [Header("Movement Script")]
    [SerializeField] MovementController movementControllerScript;

    private Transform ai1Position; // 1.AI's transform
    private Transform ai2Position; // 2.AI's transform
    private Transform ai3Position; // 3.AI's transform


    private GameObject[] aiTransformList1; // Player's transform
    private GameObject[] aiTransformList2; // Player's transform
    private GameObject[] aiTransformList3; // Player's transform

    [Header("Starting Point")]
    [SerializeField] private Transform userStartPoint; //Start point transform
    [SerializeField] private Transform ai1StartPoint; //Start point transform
    [SerializeField] private Transform ai2StartPoint; //Start point transform
    [SerializeField] private Transform ai3StartPoint; //Start point transform
    private Transform finishLine; // Finish line's transform

    [Header("Progress Bar")]
    [SerializeField] Slider progressBarUser; // Progress bar UI element
    [SerializeField] Slider progressBarAI1; // Progress bar UI element
    [SerializeField] Slider progressBarAI2; // Progress bar UI element
    [SerializeField] Slider progressBarAI3; // Progress bar UI element

    [Header("Current Level Text")]
    [SerializeField] TMP_Text CurrentLevelText;

    //Variables
    //float distanceToFinish;
    //float totalDistance;
    //float progressPercentage;

    //Flags
    private bool isSetupComplete = false;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void OnEnable()
    {
        progressBarUser.value = 0;
        progressBarAI1.value = 0;
        progressBarAI2.value = 0;
        progressBarAI3.value = 0;


        StartCoroutine(InitalSetup());
    }
    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Transform)
        {
            SetPlayerActiveVehicle();        
        }

        if (state == GameManager.GameState.Play)
        {
            SetCurrentLevelText();
        }
    }
    void Update()
    {
        if (isSetupComplete)
        {
            if(movementControllerScript.hasVehicleChanged)
            {
                SetPlayerActiveVehicle();
            }
            UserProgress();
            AI1Progress();
            AI2Progress();
            AI3Progress();
            SetAIActiveVehicle();
            SetCurrentLevelText();
        }
    }

    void SetPlayerActiveVehicle()
    {
        for (int i = 0; i < transformListArray.Length; i++)
        {
            if (transformListArray[i].activeSelf)
            {
                player = transformListArray[i].transform;
                break;
            }
        }
    }

    void SetAIActiveVehicle()
    {
        for (int i = 0; i < aiTransformList1.Length; i++)
        {
            if (aiTransformList1[i].activeSelf)
            {
                ai1Position = aiTransformList1[i].transform;
            }
        }
        for (int i = 0; i < aiTransformList2.Length; i++)
        {
            if (aiTransformList2[i].activeSelf)
            {
                ai2Position = aiTransformList2[i].transform;
            }
        }
        for (int i = 0; i < aiTransformList3.Length; i++)
        {
            if (aiTransformList3[i].activeSelf)
            {
                ai3Position = aiTransformList3[i].transform;
            }
        }
    }

    void UserProgress()
    {
        // Calculate the player's progress
        float distanceToFinish = Vector3.Distance(player.position, finishLine.position);
        float totalDistance = Vector3.Distance(userStartPoint.position, finishLine.position);

        // Calculate the progress percentage
        float progressPercentage = 1 - (distanceToFinish / totalDistance);

        // Update the progress bar value
        progressBarUser.value = progressPercentage;

        if (distanceToFinish <= 0.5f)
        {
            progressBarUser.value = 1;
        }
    }
    void AI1Progress()
    {
        // Calculate the AI's progress
        float distanceToFinish = Vector3.Distance(ai1Position.position, finishLine.position);
        float totalDistance = Vector3.Distance(ai1StartPoint.position, finishLine.position);

        // Calculate the progress percentage
        float progressPercentage = 1 - (distanceToFinish / totalDistance);

        // Update the progress bar value
        progressBarAI1.value = progressPercentage;

        if (distanceToFinish <= 0.5f)
        {
            progressBarAI1.value = 1;
        }
    }
    void AI2Progress()
    {
        // Calculate the AI's progress
        float distanceToFinish = Vector3.Distance(ai2Position.position, finishLine.position);
        float totalDistance = Vector3.Distance(ai2StartPoint.position, finishLine.position);

        // Calculate the progress percentage
        float progressPercentage = 1 - (distanceToFinish / totalDistance);

        // Update the progress bar value
        progressBarAI2.value = progressPercentage;

        if (distanceToFinish <= 0.5f)
        {
            progressBarAI2.value = 1;
        }
    }
    void AI3Progress()
    {
        // Calculate the AI's progress
        float distanceToFinish = Vector3.Distance(ai3Position.position, finishLine.position);
        float totalDistance = Vector3.Distance(ai3StartPoint.position, finishLine.position);

        // Calculate the progress percentage
        float progressPercentage = 1 - (distanceToFinish / totalDistance);

        // Update the progress bar value
        progressBarAI3.value = progressPercentage;

        if (distanceToFinish <= 0.5f)
        {
            progressBarAI3.value = 1;
        }
    }

    void GetAITransformList()
    {
        GameObject currentActiveLevel = levelManagerScript.GetCurrentActiveLevels();
        GameObject aiObj = null;
        for (int i = 0; i < currentActiveLevel.transform.childCount; i++)
        {
            if (currentActiveLevel.transform.GetChild(i).name == "AI")
            {
                aiObj = currentActiveLevel.transform.GetChild(i).gameObject;
                break;
            }
        }

        for (int i = 0; i < aiObj.transform.childCount; i++)
        {
            if (aiObj.transform.GetChild(i).gameObject.name == "AI_1")
            {
                AIController aiControllerScript1 = aiObj.transform.GetChild(i).gameObject.GetComponentInChildren<AIController>();
                aiTransformList1 = aiControllerScript1.tranformObjectsArr;
            }
            if (aiObj.transform.GetChild(i).gameObject.name == "AI_2")
            {
                AIController aiControllerScript2 = aiObj.transform.GetChild(i).gameObject.GetComponentInChildren<AIController>();
                aiTransformList2 = aiControllerScript2.tranformObjectsArr;
            }
            if (aiObj.transform.GetChild(i).gameObject.name == "AI_3")
            {
                AIController aiControllerScript3 = aiObj.transform.GetChild(i).gameObject.GetComponentInChildren<AIController>();
                aiTransformList3 = aiControllerScript3.tranformObjectsArr;
            }


        }
    }

    void GetFinishLine()
    {

        GameObject currentActiveLevel = levelManagerScript.GetCurrentActiveLevels();
        for (int i = 0; i < currentActiveLevel.transform.childCount; i++)
        {
            if (currentActiveLevel.transform.GetChild(i).name == "Win")
            {
                finishLine = currentActiveLevel.transform.GetChild(i).transform;
            }
        }
    }

    IEnumerator InitalSetup()
    {
        yield return null;
        GetFinishLine();
        SetPlayerActiveVehicle();
        GetAITransformList();
        SetAIActiveVehicle();

        isSetupComplete = true;
    }

    private void OnDisable()
    {
        isSetupComplete = false;
    }

    void SetCurrentLevelText()
    {
        //Debug.Log("Current Level : " + levelManagerScript.Int_GetCurrentActiveLevel());
        CurrentLevelText.text = "Level " + (levelManagerScript.Int_GetCurrentActiveLevel() + 1);
    }
}
