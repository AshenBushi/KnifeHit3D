using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SessionHandler : Singleton<SessionHandler>
{
    [SerializeField] private LotteryTimer _lotteryTimer;
    [Header("Screens")]
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;

    private bool _isPlayerLose;

    public event UnityAction IsSessionStarted;
    public event UnityAction IsSessionRestarted;

    private void OnEnable()
    {
        _winScreen.IsScreenDisabled += OnScreenDisabled;
        _loseScreen.IsScreenDisabled += OnScreenDisabled;
    }

    private void OnDisable()
    {
        PlayerInput.Instance.IsSessionStart -= OnSessionStart;
        _winScreen.IsScreenDisabled -= OnScreenDisabled;
        _loseScreen.IsScreenDisabled -= OnScreenDisabled;
    }

    private void Start()
    {
        PlayerInput.Instance.IsSessionStart += OnSessionStart;
    }

    public void AllowPlayerLose()
    {
        _isPlayerLose = true;
    }

    public void DisallowPlayerLose()
    {
        _isPlayerLose = false;
    }

    private void OnSessionStart()
    {
        _startScreen.Disable();

        if (GamemodManager.Instance.KnifeHitMod == 6)
            _lotteryTimer.EnableTimer();

        IsSessionStarted?.Invoke();
    }

    private void OnScreenDisabled(bool isAdShowed)
    {
        DataManager.Instance.GameData.CanShowStartAd = !isAdShowed;

        StartCoroutine(TryToShowAdRestart());
    }

    private IEnumerator TryToShowAdRestart()
    {
        if (DataManager.Instance.GameData.CanShowStartAd)
        {
            if (AdManager.Instance.ShowInterstitial())
            {
                yield return new WaitUntil(() => AdManager.Instance.IsInterstitialShowed);
            }
        }

        RestartSession();
    }

    public void CompleteLevel()
    {
        _winScreen.Win();
        _isPlayerLose = false;
    }

    public void CompleteLevelWithCutscene(float multiplierLastStep)
    {
        _winScreen.WinWithReward(multiplierLastStep);
        _isPlayerLose = false;
    }

    public void FailLevel()
    {
        _loseScreen.Lose();
        _isPlayerLose = true;
    }

    public void RestartSession()
    {
        GamemodManager.Instance.StartSession(_isPlayerLose);
        PlayerInput.Instance.Enable();
        PlayerInput.Instance.AllowTap();
        _startScreen.Enable();
        IsSessionRestarted?.Invoke();
    }
}
