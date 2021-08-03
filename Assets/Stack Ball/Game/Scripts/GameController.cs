#pragma warning disable 0649
#pragma warning disable 0414

using UnityEngine;
using Watermelon;

public class GameController : MonoBehaviour
{
    public const string LAST_LEVEL_PREFS_NAME = "lastLevel";

    private const int PLATFORMS_ON_START = 20;

    private const string CURRENT_SCORE_PREFS_NAME = "playerScore";
    private const string BEST_SCORE_PREFS_NAME = "playerBestScore";

    private static GameController instance;

    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerHitParticle playerHitParticle;
    [SerializeField] LevelController levelController;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] UIController uiController;
    [SerializeField] ProjectData projectData;
    [SerializeField] Watermelon.AudioSettings audioSettings;
    [SerializeField] AdsData adsSettings;

    [Space]
    [SerializeField] LevelDatabase levelDatabase;

    [Space]
    [SerializeField] Material defaultPlatformMaterial;
    [SerializeField] Material obstaclePlatformMaterial;

    [Space]
    [SerializeField] ParticleSystem winParticle;

    private bool revived = false;

    private int score;
    public static int Score
    {
        get { return instance.score; }
        set { instance.score = value; }
    }

    private int bestScore;
    public static int BestScore => instance.bestScore;

    private int currentLevelIndex = 0;
    public static int CurrentLevelIndex => instance.currentLevelIndex;

    public static Material DefaultPlatformMaterial => instance.defaultPlatformMaterial;
    public static Material ObstaclePlatformMaterial => instance.obstaclePlatformMaterial;

    private void Awake()
    {
        instance = this;

        score = PlayerPrefs.GetInt(CURRENT_SCORE_PREFS_NAME, 0);
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_PREFS_NAME, 0);
        levelDatabase.Init();
    }

    private void Start()
    {
        if (projectData.isOrthographicCamera)
            Camera.main.orthographic = true;

        ColorManager.Instance.RandomColorPreset();
        
        LoadLevel(PlayerPrefs.GetInt(LAST_LEVEL_PREFS_NAME, 0));
    }

    private void LoadLevel(int levelIndex)
    {
        revived = false;

        currentLevelIndex = levelIndex;

        Level level = levelDatabase.GetLevel(currentLevelIndex);
        if (level == null)
        {
            level = levelDatabase.GetLevel(Random.Range(0, levelDatabase.levels.Length));
        }

        PoolSimple platformPool = new PoolSimple("Platform", level.levelPlatformType.prefab, PLATFORMS_ON_START, true);

        levelController.Init(platformPool, level);
        levelController.SpawnPlatforms(PLATFORMS_ON_START);

        playerController.Init();

        uiController.Init(currentLevelIndex + 1, level.levelPlatformsData.Length);
        uiController.SetScoreText(score);

        cameraFollow.Init();
    }

    private void ReplayLevel()
    {
        revived = false;

        levelController.ResetLevel();
        levelController.SpawnPlatforms(PLATFORMS_ON_START);

        playerController.Init();

        uiController.SetProgress(0);
        uiController.SetScoreText(0);

        SetScore(0);

        cameraFollow.Init();
    }

    public static void Revive()
    {
        instance.uiController.DisableReviveCoroutine();
        instance.uiController.DisableGameOverPanel();

        instance.revived = true;
    }

    public static void GameOver()
    {
        if (instance.score > instance.bestScore)
        {
            instance.bestScore = instance.score;

            PlayerPrefs.SetInt(BEST_SCORE_PREFS_NAME, instance.bestScore);
        }

        bool showRevive = false;
        
        if(instance.score >= instance.adsSettings.adsFrequency.minReviveScore)
        {
            showRevive = !instance.revived && AdsManager.IsRewardBasedVideoLoaded();
        }

        instance.uiController.ShowGameOverPanel(showRevive);
    }

    public static void WinGame()
    {
        AudioController.PlaySound(instance.audioSettings.sounds.gameWinAudioClip, 1);

        PlayerPrefs.SetInt(CURRENT_SCORE_PREFS_NAME, instance.score);
        PlayerPrefs.SetInt(LAST_LEVEL_PREFS_NAME, instance.currentLevelIndex + 1);

        instance.winParticle.Play();
        
        instance.uiController.ShowWinPanel();
    }

    public static void SetScore(int score)
    {
        instance.score = score;

        PlayerPrefs.SetInt(CURRENT_SCORE_PREFS_NAME, score);
    }
}
