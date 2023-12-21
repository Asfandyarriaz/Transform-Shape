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
    [SerializeField] TMP_Text cashText;
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

        //if (state == GameState.Play)
        vehicleSelectorCanvas.SetActive(state == GameState.Play);

        //if(state != GameState.Start)
        cashText.gameObject.SetActive(state != GameState.Start);
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

    

    void DisplayCash()
    {
        cashText.text = "Cash: " + GameManager.Instance.totalCash.ToString();
    }
}
