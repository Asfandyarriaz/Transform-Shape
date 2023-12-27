using Cinemachine;
using UnityEngine;

public class VirtualCamera2Controller : MonoBehaviour
{
    [SerializeField] private LevelManager levelManagerScript;
    CinemachineVirtualCamera cam;
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }
    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.SetupGameData)
        {
            SetCameraPosition();
        }
    }
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    void SetCameraPosition()
    {
        GameObject currentActiveLevel = levelManagerScript.GetCurrentActiveLevels();
        for (int i = 0; i < currentActiveLevel.transform.childCount; i++)
        {
            if (currentActiveLevel.transform.GetChild(i).name == "Win")
            {
                cam.LookAt = currentActiveLevel.transform.GetChild(i).transform;
                cam.Follow = currentActiveLevel.transform.GetChild(i).transform;
            }
        }
    }
}
