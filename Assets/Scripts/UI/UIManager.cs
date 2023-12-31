using System;
using TMPro;
using UnityEngine;
using static GameManager;

public class UIManager : MonoBehaviour
{
    [Header("Tap To Play Canvas")]
    [SerializeField] GameObject TapToPlayCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject VehicleUnlockProgressScreen;
    [SerializeField] GameObject vehicleSelectorCanvas;
    [SerializeField] GameObject progressionScreenCanvas;
    [SerializeField] GameObject cashPanel;
    [SerializeField] TMP_Text cashText;
    [SerializeField] GameObject cashImage;
    [SerializeField] GameObject raceProgressBar;
    [SerializeField] GameObject SettingsPanel;

    [Header("Default Cash Reward")]
    [SerializeField] int defaultCashReward = 200;
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void Start()
    {
        DisplayCash();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }
    
    private void GameManagerOnGameStateChanged(GameState state)
    {
        //if(state == GameState.Start)
        TapToPlayCanvas.SetActive(state == GameState.Start);

        //if (state == GameState.Lose)
        gameOverCanvas.SetActive(state == GameState.Lose);

        //if (state == GameState.Win)
        VehicleUnlockProgressScreen.SetActive(state == GameState.NextVehicleProgress);

        //if (state == GameState.Progression Screen)
        progressionScreenCanvas.SetActive(state == GameState.ProgressionScreen);

        //if (state == GameState.Play || state == GameState.Transform)
        vehicleSelectorCanvas.SetActive(state == GameState.Play || state == GameState.Transform);

        //if(state != GameState.Start)
        cashText.gameObject.SetActive(state != GameState.Start);
        cashImage.gameObject.SetActive(state != GameState.Start);

        //if(state != GameState.Start)
        cashPanel.SetActive(state == GameState.Cash);

        //if (state == GameState.Play || state == GameState.Transform)
        raceProgressBar.SetActive(state == GameState.Play || state == GameState.Transform);

        //if (state == GameState.Settings)
        SettingsPanel.SetActive(state == GameState.Settings);
    }
    private void Update()
    {
        DisplayCash();
    }
    public void OnClickRetry()
    {
        GameManager.Instance.UpdateGameState(GameState.Start);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }
    public void OnClickNextlevel()
    {
        GameManager.Instance.UpdateGameState(GameState.Win);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }
    public void OnClickSound()
    {
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }

    public void OnClickSetting()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Settings);
    }
    public void OnClickCancelButton()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }
    public void OnClickTapToPlay()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Camera);
    }
    public void OnClickCashPanelContinue()
    {
        PlayerDataController.Instance.playerData.PlayerGold += defaultCashReward;
        PlayerDataController.Instance.Save();
        GameManager.Instance.UpdateGameState(GameState.ProgressionScreen);
        
    }
    void DisplayCash()
    {
        cashText.text =  PlayerDataController.Instance.playerData.PlayerGold.ToString();
    }
}
