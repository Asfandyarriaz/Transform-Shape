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
    /*private void OnEnable()
    {
        SetAllColoredImagesOff();
        NewVehicleUnlockProgress();
    }*/
   /* private void Start()
    {
        SetAllColoredImagesOff();
    }*/

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
        int boatLevelNeeded = 2;
        int tankLevelNeeded = 4;
        int planeLevelNeeded = 7;
        int scooterLevelNeeded = 10;
        int gliderLevelNeeded = 14;
        //Unlock Boat - Levels Needed 2 .   1,2,3 Number Notation 
        if (levelManagerScript.Int_GetCurrentActiveLevel() <= boatLevelNeeded)
        {
            vehicles[0].SetActive(true);
            StartCoroutine(FillAmountImage(boatColored, 1,0));
            //Save
            PlayerDataController.Instance.playerData.boatUnlockProgress = boatColored.fillAmount;
            PlayerDataController.Instance.Save();
        }

        ///Continue Later

       /* //Unlock Tank - Levels Needed 2 .   1,2,3 Number Notation 
        if (levelManagerScript.Int_GetCurrentActiveLevel() <= tankLevelNeeded)
        {
            vehicles[1].SetActive(true);
            StartCoroutine(FillAmountImage(tankColored, 1, 0));
            //Save
            PlayerDataController.Instance.playerData.tankUnlockProgress = tankColored.fillAmount;
            PlayerDataController.Instance.Save();
        }
        if (levelManagerScript.Int_GetCurrentActiveLevel() <= planeLevelNeeded)
        {
            vehicles[2].SetActive(true);
            StartCoroutine(FillAmountImage(planeColored, 2, 0));
            //Save
            PlayerDataController.Instance.playerData.planeUnlockProgress = planeColored.fillAmount;
            PlayerDataController.Instance.Save();
        }
        if (levelManagerScript.Int_GetCurrentActiveLevel() <= scooterLevelNeeded)
        {
            vehicles[3].SetActive(true);
            StartCoroutine(FillAmountImage(planeColored, 2, 0));
            //Save
            PlayerDataController.Instance.playerData.scooterUnlockProgress = scooterColored.fillAmount;
            PlayerDataController.Instance.Save();
        }
        if (levelManagerScript.Int_GetCurrentActiveLevel() <= gliderLevelNeeded)
        {
            vehicles[4].SetActive(true);
            StartCoroutine(FillAmountImage(gliderColored, 2, 0));
            //Save
            PlayerDataController.Instance.playerData.gliderUnlockProgress = gliderColored.fillAmount;
            PlayerDataController.Instance.Save();
        }*/


    }

    //Fill image gradually
    IEnumerator FillAmountImage(Image image, float amount, int index)
    {
        float time = 0;
        float durtaion = 10f;

        float fillAmount = 0;
        float amountAlreadyFilled = (float)PlayerDataController.Instance.playerData.boatUnlockProgress;

        SetPercentageText(amountAlreadyFilled, index);
        while (time < durtaion)
        {
            fillAmount = Mathf.Lerp(fillAmount, amount, (time / durtaion));
            if (amountAlreadyFilled <= fillAmount) { image.fillAmount = fillAmount; }
            time += Time.deltaTime;
            SetPercentageText(image.fillAmount, index);
            yield return null;
        }
        
        yield return null;
    }

    //Percentage text
    void SetPercentageText(float filledAmount, int index)
    {
        int percentage = Mathf.RoundToInt((filledAmount / 1) * 100);
        text[index].text = percentage.ToString() + "%";
    }

    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(wait);
    }
}
