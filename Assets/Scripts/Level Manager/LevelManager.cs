using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Store All levels and activates them based on current level 
/// Set number of transformable shapes and assign proper shape to them
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] levels;
    [SerializeField] public LevelProperties[] levelProperties;
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
        if (/*state == GameManager.GameState.PreStart ||*/ state == GameManager.GameState.Start)
        {
            SetCurrentLevelActive(PlayerDataController.Instance.playerData.currentLevel);
        }
        if (state == GameManager.GameState.Win)
        {

        }
        if (state == GameManager.GameState.PostWinSetupGameData)
        {
            //Update Level
            //Save
            PlayerDataController.Instance.playerData.currentLevel++;
            PlayerDataController.Instance.Save();
        }
    }

    private void Start()
    {
        SetCurrentLevelActive(PlayerDataController.Instance.playerData.currentLevel);
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
    public GameObject GetCurrentActiveLevels(int index)
    {
        return levels[index];
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
    public void ReloadScene()
    {
        // Get the current active scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneName);
    }
}
