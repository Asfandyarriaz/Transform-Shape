using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelFailedPanel : MonoBehaviour
{
    [Header("Level Failed Image")]
    [SerializeField] GameObject levelFailedImage;
    [SerializeField] Vector3 levelScale;
    [SerializeField] float levelTimeDelay = 0.2f;

    [Header("Revive Image")]
    [SerializeField] GameObject reviveImage;
    [SerializeField] Vector3 reviveScale;
    [SerializeField] float reviveTimeDelay = 0.8f;

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
        if (state == GameManager.GameState.Lose)
        {
            ResetImages();
            LeanTween.scale(levelFailedImage, levelScale, 2f).setDelay(levelTimeDelay).setEase(LeanTweenType.easeOutElastic);
            LeanTween.scale(reviveImage, reviveScale, 3f).setDelay(reviveTimeDelay).setEase(LeanTweenType.easeOutElastic);
        }
    }

    void ResetImages()
    {
        levelFailedImage.transform.localScale = new Vector3(0, 0, 0);
        reviveImage.transform.localScale = new Vector3(0, 0, 0);
    }
}
