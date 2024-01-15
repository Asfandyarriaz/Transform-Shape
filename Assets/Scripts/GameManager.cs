using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    //Global Varaibles
    public int winPosition = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       // UpdateGameState(GameState.PreStart);
        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState newState)
    {
        //if (State == newState) { return; }
        State = newState;
        switch (State)
        {
            //case GameState.PreStart: HandlePreStartState(); break;
            case GameState.Start: HandleStartState(); break;
            case GameState.Settings: HandleSettingsState(); break;
            case GameState.RemoveAds: HandleRemoveAdsState(); break;
            case GameState.Camera: HandleCameraState(); break;
            case GameState.SetupGameData: HandleSetupGameDataState(); break;
            case GameState.Play: HandlePlayState(); break;
            case GameState.Transform: HandleTransformState(); break;
            case GameState.Cash: HandleCashState(); break;
            case GameState.ProgressionScreen: HandleProgressionScreenState(); break;
            case GameState.NextVehicleProgress: HandleNextVehicleProgressState(); break;
            case GameState.Win: HandleWinState(); break;
            case GameState.PostWinSetupGameData: HandlePostWinSetupGameDataState(); break;
            case GameState.Lose: HandleLoseState(); break;
            default: throw new ArgumentOutOfRangeException(nameof(State), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }


    private void Update()
    {

    }
    /*private void HandlePreStartState()
    {
        //await Task.Delay(1);
        UpdateGameState(GameState.Start);
    }*/
    private void HandleStartState()
    {       
        winPosition = 0;
    }
    private void HandleSettingsState()
    {

    }
    private void HandleRemoveAdsState()
    {

    }
    private async void HandleCameraState()
    {
        await Task.Delay(1);
        UpdateGameState(GameState.SetupGameData);
    }
    private async void HandleSetupGameDataState()
    {
        await Task.Delay(1);
        UpdateGameState(GameState.Play);
    }
    private void HandlePlayState()
    {

    }
    private async void HandleTransformState()
    {
        await Task.Delay(1);
        UpdateGameState(GameState.Play);
    }
    private void HandleCashState()
    {

    }
    private void HandleProgressionScreenState()
    {

    }
    private void HandleNextVehicleProgressState()
    {

    }
    private async void HandleWinState()
    {
        await Task.Delay(1);
        UpdateGameState(GameState.Start);
    }
    private void HandleLoseState()
    {

    }
    private async void HandlePostWinSetupGameDataState()
    {
        await Task.Delay(1);
        UpdateGameState(GameState.Win);
    }

    public enum GameState
    {
        //PreStart,
        Start,
        Settings,
        RemoveAds,
        SetupGameData,
        Camera,
        Play,
        Transform,
        Cash,
        ProgressionScreen,
        NextVehicleProgress,
        Win,
        PostWinSetupGameData,
        Lose
    }


    #region Game Data Logic
    public void UpdateCash(int cash)
    {
        // Instance.totalCash += cash;
    }
    public void DeductCash(int cash)
    {
        if (cash <= PlayerDataController.Instance.playerData.PlayerGold)
        {
            // totalCash -= cash;
            PlayerDataController.Instance.playerData.PlayerGold -= cash;

        }
        else
        {
            Debug.LogWarning("Not Enough Cash");
        }
    }
    #endregion
}
