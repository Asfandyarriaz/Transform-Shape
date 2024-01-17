using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UIManager : MonoBehaviour
{
    [Header("     ------------ Panels ------------")]

    [Header("TapToPlay Panel")]
    [SerializeField] GameObject TapToPlayCanvas;
    [SerializeField] TMP_Text currentLevelText;

    [Header("Other Panels")]
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject VehicleUnlockProgressScreen;
    [SerializeField] GameObject vehicleSelectorCanvas;
    [SerializeField] GameObject progressionScreenCanvas;
    [SerializeField] GameObject cashPanel;
    [SerializeField] TMP_Text cashText;
    [SerializeField] GameObject cashImage;
    [SerializeField] GameObject raceProgressBar;
    [SerializeField] GameObject SettingsPanel;

    [Header("Remove Ad Panel")]
    [SerializeField] GameObject removeAdsPanel;

    [Header("Default Cash Reward")]
    [SerializeField] GameObject[] winPosition;

    [Header("Default Cash Reward")]
    [SerializeField] int defaultCashReward = 200;

    [Header("Level Manager")]
    [SerializeField] LevelManager levelManagerScript;
    [Header("Audio Manager")]
    [SerializeField] AudioManager audioManagerScript;

    [Header("     ------------ Toggle Buttons ------------")]
    //[Header("Sound Toggle")]
    [SerializeField] GameObject soundToggle;
    //[Header("Music Toggle")]
    [SerializeField] GameObject musicToggle;
    //[Header("Vibration Toggle")]
    [SerializeField] GameObject vibrationToggle;

    [Header("Name Text")]
    [SerializeField] TMP_InputField nameText;
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void Start()
    {     
        SetPlayerNameText();
        DisplayCash();
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        //------------ Tap To Play Panel ------------
        //if(state == GameState.Start)
        TapToPlayCanvas.SetActive(state == GameState.Start);
        if (state == GameState.Start)
            SetCurrentLevelText();
        //------------------------

        //if (state == GameState.Lose)
        gameOverCanvas.SetActive(state == GameState.Lose);

        //if (state == GameState.Win)
        VehicleUnlockProgressScreen.SetActive(state == GameState.NextVehicleProgress);

        //if (state == GameState.Progression Screen)
        progressionScreenCanvas.SetActive(state == GameState.ProgressionScreen);

        //if (state == GameState.Play || state == GameState.Transform)
        vehicleSelectorCanvas.SetActive(state == GameState.Play || state == GameState.Transform);

        //if(state != GameState.Start)
        cashText.gameObject.SetActive(state == GameState.Start || state == GameState.Cash || state == GameState.ProgressionScreen);
        cashImage.gameObject.SetActive(state == GameState.Start || state == GameState.Cash || state == GameState.ProgressionScreen);

        //------------ Cash Panel ------------
        //if(state != GameState.Start)
        cashPanel.SetActive(state == GameState.Cash);
        if (state == GameState.Cash)
            DisplayWinPosition();
        //------------------------

        //if (state == GameState.Play || state == GameState.Transform)
        raceProgressBar.SetActive(state == GameState.Play || state == GameState.Transform);

        //------------ Setting Panel ------------
        //if (state == GameState.Settings)
        SettingsPanel.SetActive(state == GameState.Settings);
        if (state == GameState.Settings)
        {
            SetSoundToggle();
            SetMusicToggle();
            SetVibrationToggle();
        }
        //------------------------

        //------------ Remove Ad Panel ------------
        //if(state != GameState.Start)
        removeAdsPanel.SetActive(state == GameState.RemoveAds);
        //------------------------
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
        GameManager.Instance.UpdateGameState(GameState.PostWinSetupGameData);
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
    public void OnClickRemoveAdsButton()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.RemoveAds);
    }
    public void OnClickNoThanksButton()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }

    //----------Sound Toggle----------
    //TODO: Mute Sound Logic
    public void OnClickSoundToggle()
    {
        Toggle uiToggle = soundToggle.GetComponent<Toggle>();

        if (uiToggle.isOn == true)
        {
            audioManagerScript.MuteMusic(false);
            PlayerDataController.Instance.playerData.isSoundAllow = true;
            PlayerDataController.Instance.Save();
        }
        if (uiToggle.isOn == false)
        {
            audioManagerScript.MuteMusic(true);
            PlayerDataController.Instance.playerData.isSoundAllow = false;
            PlayerDataController.Instance.Save();
        }
    }
    void SetSoundToggle()
    {
        Toggle uiToggle = soundToggle.GetComponent<Toggle>();
        if(PlayerDataController.Instance.playerData.isSoundAllow == false)
        {
            uiToggle.isOn = false;
        }
        else
        {
            uiToggle.isOn = true;
        }
    }
        
    //----------Music Toggle----------
    //TODO: Mute Music Logic
    public void OnClickMusicToggle()
    {
        Toggle uiToggle = musicToggle.GetComponent<Toggle>();

        if (uiToggle.isOn == true)
        {
            audioManagerScript.MuteMusic(false);
            PlayerDataController.Instance.playerData.isMusicAllow = true;
            PlayerDataController.Instance.Save();
        }
        if (uiToggle.isOn == false)
        {
            audioManagerScript.MuteMusic(true);
            PlayerDataController.Instance.playerData.isMusicAllow = false;
            PlayerDataController.Instance.Save();
        }

    }
    void SetMusicToggle()
    {
        Toggle uiToggle = musicToggle.GetComponent<Toggle>();
        if (PlayerDataController.Instance.playerData.isMusicAllow == false)
        {
            uiToggle.isOn = false;
        }
        else
        {
            uiToggle.isOn = true;
        }
    }

    //----------Vibration Toggle----------
    //TODO: Mute Vibration Logic
    public void OnClickVibrationToggle()
    {
        Toggle uiToggle = vibrationToggle.GetComponent<Toggle>();

        if (uiToggle.isOn == true)
        {
            audioManagerScript.MuteMusic(false);
            PlayerDataController.Instance.playerData.isVibrationAllow = true;
            PlayerDataController.Instance.Save();
        }
        if (uiToggle.isOn == false)
        {
            audioManagerScript.MuteMusic(true);
            PlayerDataController.Instance.playerData.isVibrationAllow = false;
            PlayerDataController.Instance.Save();
        }
    }
    void SetVibrationToggle()
    {
        Toggle uiToggle = vibrationToggle.GetComponent<Toggle>();
        if (PlayerDataController.Instance.playerData.isVibrationAllow == false)
        {
            uiToggle.isOn = false;
        }
        else
        {
            uiToggle.isOn = true;
        }
    }
    public void ReadStringInput(string name)
    {
        PlayerDataController.Instance.playerData.playerName = name;
        PlayerDataController.Instance.Save();
    }
    public void SetPlayerNameText()
    {
        if(PlayerDataController.Instance.playerData.playerName != null)
        nameText.text = PlayerDataController.Instance.playerData.playerName;
    }
    void DisplayCash()
    {
        cashText.text = PlayerDataController.Instance.playerData.PlayerGold.ToString();
        //cashText.text = "100";
    }

    //Display Different Position Object Based On Player Win Position
    void DisplayWinPosition()
    {
        //Turn off all objects
        for (int i = 0; i < winPosition.Length; i++)
        {
            winPosition[i].gameObject.SetActive(false);
        }

        winPosition[GameManager.Instance.winPosition - 1].SetActive(true);
    }
    async void SetCurrentLevelText()
    {
        await Task.Delay(1);
        currentLevelText.text = "Level " + (levelManagerScript.Int_GetCurrentActiveLevel() + 1);
    }
}
