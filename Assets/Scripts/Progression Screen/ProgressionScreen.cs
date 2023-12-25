using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
/// <summary>
/// This script controls the functionality of Progression Screen
/// The buttons functionality is implemented in this script
/// Purpose of this script is to display what upgrades can be done according to the gold/cash in hand
/// and update save values with vehicle upgrade level
/// </summary>
/// 

//TODO: HardCoded Values in this code Fix 

//Load Data is called in this script
public class ProgressionScreen : MonoBehaviour
{
    [Header("ORDER MATTERS")]                                         //IMP !! Same as Array
    [SerializeField] private VehicleProperties[] vehicleProperties;  //1.Character  //4.Scooter
                                                                     //2.Car        //5.Boat
                                                                     //3.Tank       //6.Airplane
    [SerializeField] private GameObject[] upgradeButtons;
    [SerializeField] private GameObject[] costImage;
    [SerializeField] private GameObject[] watchAdButtons;
    [SerializeField] private TMP_Text[] upgradeButtonsText;
    [SerializeField] private GameObject[] currentLevelStars;

    [Header("Upgrade Cost Setting")]
    [SerializeField] private int baseCost;
    [SerializeField] private float scalingFactor;

    [Header("Count Varaible To Tell Number of Button Active")]
    [SerializeField] int count = -1;

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
        if (state == GameManager.GameState.Start)
            SetAllObjectsOff();

        if (state == GameManager.GameState.ProgressionScreen)
        {
            Upgradeable();
        }
    }

    private void Start()
    {
        //Load Data

    }
    private void Update()
    {
        Debug.Log("Count : " + count);
    }

    //TODO: FIX BUG !!
    //Bug: On start 3 upgrade options presented
    //If upgrade one thats its cost crosses the total amount 
    //it will disappear 

    //TODO: Implement incremental speed changes (Completed)
    //TODO: Implement Upgraded speed to all the vehicles (Completed) 
    //TODO: Implement Save Manager (Completed)

    /// <summary>
    /// On start show 3 upgrade options 
    /// with current level, cost and increment speed
    /// all options are dynamically updated as the upgrade progresess 
    /// How this code works: First show all those upgrade that can be bought with current cash 
    /// if user cant afford then replace the buy option with watch ads option to upgrade or skip 
    /// if the upgrade cost crosses the current cash show another upgrade that user can afford 
    /// else show an upgrade running sequentially untill a non active upgrade is found 
    /// </summary>
    void Upgradeable()
    {
        ResetInteractable();
        float cost = 0;

        for (int i = 0; i < vehicleProperties.Length; i++)
        {
            cost = GetCurrentCost(vehicleProperties[i].currentUpgradeLevel);
            if (cost <= PlayerDataController.Instance.playerData.PlayerGold)
            {             
                upgradeButtonsText[i].text = cost.ToString();
                SetObjects(i);
                count++;
                if (count == 2)
                    break;
            }
        }
        if (count < 2)
        {
            for (int i = 0; i < vehicleProperties.Length; i++)
            {
                if (count < 2)
                {
                    SetObjects(i, true);
                }
                else if(count == 3)
                    break;
            }
        }
        if (count == 2)
        {
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                if (upgradeButtons[i].activeSelf)
                {
                    if (GetCurrentCost(vehicleProperties[i].currentUpgradeLevel) > PlayerDataController.Instance.playerData.PlayerGold)
                    {
                        costImage[i].SetActive(false);
                        watchAdButtons[i].SetActive(true);
                        Button button = upgradeButtons[i].gameObject.GetComponent<Button>();
                        button.interactable = false;
                    }
                }
            }
        }
    }


    //Cost = basecost * (scaling Factor raised to power current level of vehicle)
    int GetCurrentCost(int currentLevel)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(scalingFactor, currentLevel - 1));
    }

    void SetAllObjectsOff()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].SetActive(false);
            watchAdButtons[i].SetActive(false);
        }
    }
    void SetObjects(int index)
    {
        upgradeButtons[index].SetActive(true);
        for (int i = 0; i < vehicleProperties[index].currentUpgradeLevel - 1; i++)
        {
            if (!currentLevelStars[index].transform.GetChild(i).gameObject.activeSelf)
            {
                currentLevelStars[index].transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    void SetObjects(int index, bool check)
    {
        if (!upgradeButtons[index].activeSelf)
        {
            upgradeButtons[index].SetActive(true);
            for (int i = 0; i < vehicleProperties[index].currentUpgradeLevel - 1; i++)
            {
                if (!currentLevelStars[index].transform.GetChild(i).gameObject.activeSelf)
                {
                    currentLevelStars[index].transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            count++;
        }
    }

    void ResetInteractable()
    {
        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            Button button = upgradeButtons[i].gameObject.GetComponent<Button>();
            button.interactable = true;
        }
    }

    //Follow Order Scheme of array as declared in hierarchy
    //TODO: Lerp To Disappear
    public void OnClickUpgradeCharacter()
    {
        GameManager.Instance.DeductCash(GetCurrentCost(vehicleProperties[0].currentUpgradeLevel));
        vehicleProperties[0].currentUpgradeLevel++;
        upgradeButtons[0].SetActive(false);
        count--;

        //Save
        PlayerDataController.Instance.playerData.characterLevel = vehicleProperties[0].currentUpgradeLevel;
        PlayerDataController.Instance.Save();
        Upgradeable();
    }
    public void OnClickUpgradeCar()
    {
        GameManager.Instance.DeductCash(GetCurrentCost(vehicleProperties[1].currentUpgradeLevel));
        vehicleProperties[1].currentUpgradeLevel++;
        upgradeButtons[1].SetActive(false);
        count--;

        //Save
        PlayerDataController.Instance.playerData.carLevel = vehicleProperties[1].currentUpgradeLevel;
        PlayerDataController.Instance.Save();
        Upgradeable();
    }
    public void OnClickUpgradeTank()
    {
        GameManager.Instance.DeductCash(GetCurrentCost(vehicleProperties[2].currentUpgradeLevel));
        vehicleProperties[2].currentUpgradeLevel++;
        upgradeButtons[2].SetActive(false);
        count--;

        //Save
        PlayerDataController.Instance.playerData.tankLevel = vehicleProperties[2].currentUpgradeLevel;
        PlayerDataController.Instance.Save();
        Upgradeable();
    }
    public void OnClickUpgradeScooter()
    {
        GameManager.Instance.DeductCash(GetCurrentCost(vehicleProperties[3].currentUpgradeLevel));
        vehicleProperties[3].currentUpgradeLevel++;
        upgradeButtons[3].SetActive(false);
        count--;

        //Save
        PlayerDataController.Instance.playerData.scooterLevel = vehicleProperties[3].currentUpgradeLevel;
        PlayerDataController.Instance.Save();
        Upgradeable();
    }
    public void OnClickUpgradeBoat()
    {
        GameManager.Instance.DeductCash(GetCurrentCost(vehicleProperties[4].currentUpgradeLevel));
        vehicleProperties[4].currentUpgradeLevel++;
        upgradeButtons[4].SetActive(false);
        count--;

        //Save
        PlayerDataController.Instance.playerData.boatLevel = vehicleProperties[4].currentUpgradeLevel;
        PlayerDataController.Instance.Save();
        Upgradeable();
    }
    public void OnClickUpgradeAirplane()
    {
        GameManager.Instance.DeductCash(GetCurrentCost(vehicleProperties[5].currentUpgradeLevel));
        vehicleProperties[5].currentUpgradeLevel++;
        upgradeButtons[5].SetActive(false);
        count--;

        //Save
        PlayerDataController.Instance.playerData.airplaneLevel = vehicleProperties[5].currentUpgradeLevel;
        PlayerDataController.Instance.Save();
        Upgradeable();
    }
    public void OnClickNextButton()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
    }
}
