using TMPro;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    [Header("Player Marker")]
    [SerializeField] TMP_Text playerName;

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
        if (state == GameManager.GameState.Play)
        {
            if (PlayerDataController.Instance.playerData.playerName != null)
            {
                playerName.text = PlayerDataController.Instance.playerData.playerName;
            }
            else
            {
                playerName.text = "YOU";
            }
        }
    }
}
