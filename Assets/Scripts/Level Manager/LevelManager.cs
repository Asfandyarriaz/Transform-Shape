using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store All levels and activates them based on current level 
/// Set number of transformable shapes and assign proper shape to them
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] levels;
    [SerializeField] LevelProperties[] levelProperties;
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    //TODO: Get current level from save system
    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Start)
        {
            levels[0].SetActive(true);
        }
    }
}
