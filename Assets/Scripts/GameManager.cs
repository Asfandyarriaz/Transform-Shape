using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

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
    private void HandleWinState()
    {

    }
    private void HandleLoseState()
    {
       
    }

    public enum GameState
    {
        Start,
        Camera,
        Play,
        Transform,
        Win,
        Lose
    }
}
