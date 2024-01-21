using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{

    AIController aiControllerScript;

    //Level Manager
    [Header("Level Manager And Properties")]
    [SerializeField] public LevelManager levelManagerScript;
    [SerializeField] public LevelProperties[] levelProperties;

    [Header("Vehicles Properties")]
    [SerializeField] VehicleProperties[] vehicleProperties;

    //List
    public List<string> activeVehicles = new List<string>();

    //Variables
    int vehicleIndex;
    public GameObject fastestVehicleInCurrentLevel;

    //Flags

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

            //Debug.Log("Fastest Vehicle In Level : " + fastestVehicleInCurrentLevel.name);
        }

        if (state == GameManager.GameState.Play)
        {
            //Debug.Log("Number of times this statement run : ");
            CurrentActiveVehicles();
            FindFastestVehicle();
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
        aiControllerScript = GetComponent<AIController>();
    }

    //TODO:Check Performance hit this function keeps running in update. Good practice ? 
    #endregion


    //Find current active vehicles in a level
    void CurrentActiveVehicles()
    {
        int currentLevel = levelManagerScript.Int_GetCurrentActiveLevel();

        //List has been filled for current vehicle pool in a level
        for (int i = 0; i < levelProperties[currentLevel].shapeNames.Length; i++)
        {
            activeVehicles.Add(levelProperties[currentLevel].shapeNames[i]);
        }
    }

    //Find the best available vehicle based on speed
    void FindFastestVehicle()
    {
        float highest = 0;
        string fastestVehicle = "Empty";

        for (int i = 0; i < activeVehicles.Count; i++)
        {
            for (int j = 0; j < vehicleProperties.Length; j++)
            {
                if (activeVehicles[i].Equals(vehicleProperties[j].name))
                {
                    if (highest <= vehicleProperties[j].speed)
                    {
                        highest = vehicleProperties[j].speed;
                        fastestVehicle = vehicleProperties[j].name;
                    }
                }
            }
        }

        if (aiControllerScript != null)
        {
            if (aiControllerScript.tranformObjectsArr != null)
            {
                //Set Fastest Vehicle
                for (int i = 0; i < aiControllerScript.tranformObjectsArr.Length; i++)
                {
                    if (fastestVehicle.Equals(aiControllerScript.tranformObjectsArr[i].name))
                    {
                        fastestVehicleInCurrentLevel = aiControllerScript.tranformObjectsArr[i];
                    }
                }
            }
            else
            {
                Debug.LogWarning("aiControllerScript.tranformObjectsArr == null");
                FindFastestVehicle();
            }
        }
        else
        {
            Debug.LogWarning("aiControllerScript == null");            
        }
    }
}

