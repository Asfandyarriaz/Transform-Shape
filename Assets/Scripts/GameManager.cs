using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    //Global Varaibles
    public int totalCash;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState newState)
    {
        //if (State == newState) { return; }
        State = newState;
        switch (State)
        {
            case GameState.Start: HandleStartState(); break;
            case GameState.Camera: HandleCameraState(); break;
            case GameState.Play: HandlePlayState(); break;
            case GameState.Transform: HandleTransformState(); break;
            case GameState.ProgressionScreen: HandleProgressionScreenState(); break;
            case GameState.NextVehicleProgress: HandleNextVehicleProgressState(); break;
            case GameState.Win: HandleWinState(); break;
            case GameState.Lose: HandleLoseState(); break;
            default: throw new ArgumentOutOfRangeException(nameof(State), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    

    private void HandleStartState()
    {
        
    }
    private async void HandleCameraState()
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
    private void HandleProgressionScreenState()
    {
        
    }
    private void HandleWinState()
    {

    }
    private void HandleLoseState()
    {

    }
    private void HandleNextVehicleProgressState()
    {

    }
    public enum GameState
    {
        Start,
        Camera,
        Play,
        Transform,
        NextVehicleProgress,
        ProgressionScreen,
        Win,
        Lose
    }


    #region Game Data Logic
    public void UpdateCash(int cash)
    {
        Instance.totalCash += cash;
    }
    public void DeductCash(int cash)
    {
        if(cash <= totalCash)
        {
            totalCash -= cash;
        }
        else
        {
            Debug.LogWarning("Not Enough Cash");
        }
    }
    #endregion
}
