#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Watermelon;

public class UIController : MonoBehaviour
{
    private const float REVIVE_TIME = 5.0f;

    [SerializeField] PlayerController playerController;

    [Space]
    [SerializeField] CanvasGroup levelProgressCanvasGroup;
    [SerializeField] Image levelProgressImage;
    [SerializeField] RectTransform levelProgressArrow;
    
    [SerializeField] Text scoreText;

    [SerializeField] GameObject gameOverPanel;

    [SerializeField] GameObject defaultGameOverPanel;
    [SerializeField] GameObject reviveGameOverPanel;

    [SerializeField] Text resultScoreText;
    [SerializeField] Text bestScoreText;
    
    [SerializeField] GameObject winPanel;
    [SerializeField] Text levelText;
    
    [SerializeField] Image reviveFillBar;
    [SerializeField] GameObject skipReviveButton;

    private int totalPlatformsCount;

    private float levelProgressBarWidth;

    private Coroutine reviveCoroutine;
    private Coroutine fillCoroutine;

    private void Awake()
    {
        RectTransform progressBarArrowContainer = (RectTransform)levelProgressArrow.parent;
        levelProgressBarWidth = progressBarArrowContainer.rect.width;
    }

    private void OnEnable()
    {
        playerController.onPlayerDestructPlatform += OnPlayerDestructPlatform;
        playerController.onGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        playerController.onPlayerDestructPlatform -= OnPlayerDestructPlatform;
        playerController.onGameStarted -= OnGameStarted;
    }

    public void Init(int levelID, int totalPlatforms)
    {
        this.totalPlatformsCount = totalPlatforms;

        SetProgress(0);
    }

    public void DisableGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void HideLevelProgress()
    {
        scoreText.gameObject.SetActive(false);
        levelProgressCanvasGroup.DOFade(0, 0.3f);
    }

    public void ShowLevelProgress()
    {
        levelProgressCanvasGroup.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);

        levelProgressCanvasGroup.alpha = 0;
        levelProgressCanvasGroup.DOFade(1, 0.5f);
    }

    private IEnumerator ShowRevivePanel()
    {
        reviveFillBar.fillAmount = 1;

        bool noThanksIsActive = false;

        for (float f = 0; f <= REVIVE_TIME; f += UnityEngine.Time.deltaTime)
        {
            if (!noThanksIsActive && f >= 2.0f)
                skipReviveButton.SetActive(true);

            reviveFillBar.fillAmount = 1 - Mathf.Lerp(0, 1, f / REVIVE_TIME);

            yield return null;
        }

        reviveGameOverPanel.SetActive(false);

        ShowGameOverPanel(false);
    }

    public void DisableReviveCoroutine()
    {
        if (reviveCoroutine != null)
            StopCoroutine(reviveCoroutine);

        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);
    }

    public void SkipRevive()
    {
        if (reviveCoroutine != null)
            StopCoroutine(reviveCoroutine);

        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        reviveGameOverPanel.SetActive(false);

        ShowGameOverPanel(false);
    }

    public void ShowGameOverPanel(bool showRevive)
    {
        /*HideLevelProgress();

        gameOverPanel.SetActive(true);
        reviveGameOverPanel.SetActive(false);
        defaultGameOverPanel.SetActive(false);
        skipReviveButton.SetActive(false);

        if (showRevive)
        {
            reviveGameOverPanel.SetActive(true);

            reviveCoroutine = StartCoroutine(ShowRevivePanel());
        }
        else
        {
            defaultGameOverPanel.SetActive(true);

            resultScoreText.text = "0";
            bestScoreText.text = GameController.BestScore.ToString();

            fillCoroutine = StartCoroutine(FillTextSmooth(GameController.Score, 1));
        }*/
        
        SessionHandler.Instance.FailLevel();
        HideLevelProgress();
    }

    public void ShowWinPanel()
    {
        /*HideLevelProgress();

        winPanel.SetActive(true);

        levelText.text = "Level " + (GameController.CurrentLevelIndex + 1);*/
        
        SessionHandler.Instance.CompleteLevel();
        HideLevelProgress();
    }

    private IEnumerator FillTextSmooth(int value, float time)
    {
        for(float state = 0; state < 1; state += UnityEngine.Time.deltaTime / time)
        {
            resultScoreText.text = Mathf.RoundToInt(Mathf.Lerp(0, value, state)).ToString();

            yield return null;
        }

        resultScoreText.text = value.ToString();
    }
    
    private void OnGameStarted()
    {
        ShowLevelProgress();
    }

    private void OnPlayerDestructPlatform(Platform platform)
    {
        SetProgress(1 - (float)platform.platformIndex / totalPlatformsCount);

        SetScoreText(GameController.Score);
    }

    public void SetProgress(float value)
    {
        levelProgressImage.fillAmount = value;
        levelProgressArrow.anchoredPosition = new Vector2(Mathf.Lerp(0, levelProgressBarWidth, value), levelProgressArrow.anchoredPosition.y);
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}
