using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUnlockProgress : MonoBehaviour
{
    [Header("IMP DONT CHANGE ORDER")]
    [Header("level Manager")]
    [SerializeField] private LevelManager levelManagerScript;

    [Header("Vehicles")]
    [SerializeField] GameObject[] vehicles;  //1.Boat 2.Tank 3.Plane 4.Scooter 5.Glider

    [Header("Text For Vehicles")]
    [SerializeField] TMP_Text[] text;        //1.Boat 2.Tank 3.Plane 4.Scooter 5.Glider

    [Header("Colored Images")]
    [SerializeField] Image tankColored;
    [SerializeField] Image gliderColored;
    [SerializeField] Image planeColored;
    [SerializeField] Image boatColored;
    [SerializeField] Image scooterColored;

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
        if (state == GameManager.GameState.NextVehicleProgress)
        {
            SetAllImagesOff();
            StartCoroutine(Wait(0.1f));
            NewVehicleUnlockProgress();        
        }
        
    }
    void SetAllImagesOff()
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].SetActive(false);
        }
    }

    //Call Relevant Vehicle Unlock Logic Based On Next Level

    //Level 0,1,2 Number Notation
    //1 = 100/2
    //2 = 100/3 ....
    void NewVehicleUnlockProgress()
    {
        int boatLevelNeeded = 1;     //After 2 Levels
        int tankLevelNeeded = 4;     //After Further 3 Levels
        int planeLevelNeeded = 7;    //After Further 3 Levels
        int gliderLevelNeeded = 10;   //After Further 3 Levels
        int scooterLevelNeeded = 13; //After Further 3 Levels


        Debug.Log("Inside Vehicle Unlock Progress");
        Debug.Log("Current Active Level : " + levelManagerScript.Int_GetCurrentActiveLevel());
        Debug.Log("Boat Level Needed : " + boatLevelNeeded);
        Debug.Log("Is Current Level " + levelManagerScript.Int_GetCurrentActiveLevel() + " < " + 2);
        //Unlock Boat - Levels Needed 2 .   1,2,3 Number Notation 
        if (levelManagerScript.Int_GetCurrentActiveLevel() <= boatLevelNeeded && levelManagerScript.Int_GetCurrentActiveLevel() < 2)
        {
            //Save
            PlayerDataController.Instance.playerData.boatUnlockProgress += 0.5f;
            PlayerDataController.Instance.Save();

            SetAllImagesOff();
            vehicles[0].SetActive(true);
            StartCoroutine(FillAmountImage(boatColored, PlayerDataController.Instance.playerData.boatUnlockProgress,0));
        }

        ///Continue Later
        //Unlock Tank - Levels Needed 2 .   1,2,3 Number Notation 
        else if (levelManagerScript.Int_GetCurrentActiveLevel() <= tankLevelNeeded && levelManagerScript.Int_GetCurrentActiveLevel() >= boatLevelNeeded)
        {
            //Save
            PlayerDataController.Instance.playerData.tankUnlockProgress += 0.33f;
            PlayerDataController.Instance.Save();

            SetAllImagesOff();
            vehicles[1].SetActive(true);
            StartCoroutine(FillAmountImage(tankColored, PlayerDataController.Instance.playerData.tankUnlockProgress, 1));
        }
        //Unlock Plane
        else if (levelManagerScript.Int_GetCurrentActiveLevel() <= planeLevelNeeded && levelManagerScript.Int_GetCurrentActiveLevel() >= tankLevelNeeded)
        {
            //Save
            PlayerDataController.Instance.playerData.planeUnlockProgress += 0.33f;
            PlayerDataController.Instance.Save();

            SetAllImagesOff();
            vehicles[2].SetActive(true);
            StartCoroutine(FillAmountImage(planeColored, PlayerDataController.Instance.playerData.planeUnlockProgress, 2));
        }
        else if (levelManagerScript.Int_GetCurrentActiveLevel() <= gliderLevelNeeded && levelManagerScript.Int_GetCurrentActiveLevel() >= planeLevelNeeded)
        {
            //Save
            PlayerDataController.Instance.playerData.gliderUnlockProgress += 0.33f;
            PlayerDataController.Instance.Save();

            SetAllImagesOff();
            vehicles[3].SetActive(true);
            StartCoroutine(FillAmountImage(gliderColored, PlayerDataController.Instance.playerData.gliderUnlockProgress, 3));
        }
        else if (levelManagerScript.Int_GetCurrentActiveLevel() <= scooterLevelNeeded && levelManagerScript.Int_GetCurrentActiveLevel() >= gliderLevelNeeded)
        {
            //Save
            PlayerDataController.Instance.playerData.scooterUnlockProgress += 0.33f;
            PlayerDataController.Instance.Save();

            SetAllImagesOff();
            vehicles[4].SetActive(true);
            StartCoroutine(FillAmountImage(scooterColored, PlayerDataController.Instance.playerData.scooterUnlockProgress, 4));
        }
    }

    //Fill image gradually
    IEnumerator FillAmountImage(Image image, float amount, int index)
    {
        float time = 0;
        float durtaion = 10f;
        float fillAmount = 0;    
        
        //SetPercentageText(amountAlreadyFilled, index);
        while (time < durtaion)
        {
            fillAmount = Mathf.Lerp(fillAmount, amount, (time / durtaion));
            image.fillAmount = fillAmount;
            time += Time.deltaTime;
            SetPercentageText(image.fillAmount, index);
            yield return null;
        }       
        yield return null;

        if(fillAmount >= 99)
        {
            fillAmount = 1;
            image.fillAmount = fillAmount;
            SetPercentageText(image.fillAmount, index);
        }
    }

    //Percentage text
    void SetPercentageText(float filledAmount, int index)
    {
        //int percentage = Mathf.RoundToInt((filledAmount / 1) * 100);
        float percentage = Mathf.Ceil((filledAmount / 1) * 100);
        text[index].text = percentage.ToString() + "%";
    }

    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(wait);
    }
}
