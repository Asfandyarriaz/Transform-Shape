using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class UIManager : MonoBehaviour
{
    [Header("Tap To Play Canvas")]
    [SerializeField] Canvas playCanvas;

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
        playCanvas.gameObject.SetActive(state == GameState.Start);
    }
}
