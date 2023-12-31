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
    [SerializeField] GameObject[] transformButtons;
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
        if (state == GameManager.GameState.Start)
        {
            Debug.Log("Current Level : " + PlayerDataController.Instance.playerData.currentLevel);
            SetCurrentLevelActive(PlayerDataController.Instance.playerData.currentLevel);
        }
        if (state == GameManager.GameState.Win)
        {
            PlayerDataController.Instance.playerData.currentLevel++;
            PlayerDataController.Instance.Save();
        }
    }

    void SetCurrentLevelActive(int index)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(false);

        }

        for (int i = 0; i < levels.Length; i++)
        {
            if (i == index)
            {
                levels[i].SetActive(true);
                break;
            }
        }
        SetButtons(index);
    }

    public GameObject GetCurrentActiveLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if(levels[i].activeSelf)
            {
                return levels[i];
            }
        }
        return null;
    }
    public int Int_GetCurrentActiveLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].activeSelf)
            {
                return i;
            }
        }
        return 0;
    }


    //Check buttons based on name and activate them
    void SetButtons(int currentLevel)
    {
        int totalButtons = levelProperties[currentLevel].numberOfShapes;
        for (int i = 0; i < transformButtons.Length; i++)
        {
            transformButtons[i].SetActive(false);
        }


        for (int i = 0; i < totalButtons; i++)
        {
            for (int j = 0; j < transformButtons.Length; j++)
            {
                if (levelProperties[currentLevel].shapeNames[i].Equals(transformButtons[j].name))
                {
                    transformButtons[j].SetActive(true);
                }
            }
        }
    }
}
