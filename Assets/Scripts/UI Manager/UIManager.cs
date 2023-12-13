using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class UIManager : MonoBehaviour
{
    [Header("Tap To Play Canvas")]
    [SerializeField] GameObject TapToPlayCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject victoryCanvas;
    [SerializeField] GameObject vehicleSelectorCanvas;
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
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

        vehicleSelectorCanvas.SetActive(state == GameState.Play);
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
}
