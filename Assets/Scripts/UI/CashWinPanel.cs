using UnityEngine;

public class CashWinPanel : MonoBehaviour
{
    [Header("Level Completed Image")]
    [SerializeField] GameObject levelCompletedImage;
    [SerializeField] Vector3 levelScale;
    [SerializeField] float levelTimeDelay = 0.2f;

    [Header("Star Image")]
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;
    [SerializeField] GameObject star3;
    [SerializeField] Vector3 starScale;
    [SerializeField] float starTimeDelay = 0.2f;
    [SerializeField] float starDelayOffset = 0.2f;
    [SerializeField] float starAnimationTime = 2f;


    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void Start()
    {
        levelCompletedImage.transform.localScale = new Vector3(0, 0, 0);
    }
    //TODO: Get current level from save system
    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Cash)
        {
            ResetImages();
            LeanTween.scale(levelCompletedImage, levelScale, 2f).setDelay(levelTimeDelay).setEase(LeanTweenType.easeOutElastic);
            PopUpStars();
        }
    }

    void PopUpStars()
    {
        LeanTween.scale(star1, starScale, starAnimationTime).setDelay(starTimeDelay + starDelayOffset * 1).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star2, starScale, starAnimationTime).setDelay(starTimeDelay + starDelayOffset * 2).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, starScale, starAnimationTime).setDelay(starTimeDelay + starDelayOffset * 3).setEase(LeanTweenType.easeOutElastic);
    }

    void ResetImages()
    {
        levelCompletedImage.transform.localScale = new Vector3(0, 0, 0);
        star1.transform.localScale = new Vector3(0,0,0);
        star2.transform.localScale = new Vector3(0, 0, 0);
        star3.transform.localScale = new Vector3(0, 0, 0);
    }
}
