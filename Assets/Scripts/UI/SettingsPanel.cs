using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] GameObject backGroundImage;
    [SerializeField] GameObject cancelButton;

    [Header("Settings")]
    [SerializeField] Vector3 scale;
    [SerializeField] private float animationTime;
    [SerializeField] private float delay;
    [SerializeField] private Vector3 endScale = new Vector3(0f, 0f, 0f);

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void OnEnable()
    {
        ResetImages();
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Settings)
        {
            StartAnimation();
        }
    }

    void ResetImages()
    {
        backGroundImage.transform.localScale = new Vector3(0, 0, 0);
        cancelButton.transform.localScale = new Vector3(0, 0, 0);
    }

    void StartAnimation()
    {
        LeanTween.scale(backGroundImage, scale, animationTime).setDelay(delay).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(cancelButton, scale, animationTime).setDelay(delay).setEase(LeanTweenType.easeOutSine);
    }
    public void OnEndAnimation()
    {
        LeanTween.scale(backGroundImage, endScale, animationTime).setDelay(delay).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(cancelButton, endScale, animationTime).setDelay(delay).setEase(LeanTweenType.easeOutSine);
        LeanTween.delayedCall(animationTime, () => SwitchToStartState());
    }

    public void SwitchToStartState()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }
}
