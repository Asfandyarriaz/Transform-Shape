using System;
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
            case GameState.Play: HandlePlayState(); break;
            case GameState.Win: HandleWinState(); break;
            case GameState.Lose: HandleLoseState(); break;
            default: throw new ArgumentOutOfRangeException(nameof(State), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleStartState()
    {

    }
    private void HandlePlayState()
    {

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
        Play,
        Win,
        Lose
    }
}
