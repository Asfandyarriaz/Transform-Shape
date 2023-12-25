using System;
using TMPro;
using UnityEngine;
using static GameManager;

public class UIManager : MonoBehaviour
{
    [Header("Tap To Play Canvas")]
    [SerializeField] GameObject TapToPlayCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject victoryCanvas;
    [SerializeField] GameObject vehicleSelectorCanvas;
    [SerializeField] GameObject progressionScreenCanvas;
    [SerializeField] GameObject cashPanel;
    [SerializeField] TMP_Text cashText;
    [SerializeField] GameObject cashImage;
    [SerializeField] GameObject raceProgressBar;

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
        victoryCanvas.SetActive(state == GameState.Win);

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
    public void OnClickSound()
    {
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
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
