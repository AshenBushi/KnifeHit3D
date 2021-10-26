using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SessionHandler : Singleton<SessionHandler>
{
    [SerializeField] private LotteryTimer _lotteryTimer;
    [Header("Screens")]
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private ContinueScreen _continueScreen;
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

    public void CompleteLevel()
    {
        _winScreen.Win();
        _isPlayerLose = false;
    }

    public void CompleteLevelWithCutscene(float multiplierLastStep)
    {
        MetricaManager.SendEvent("arrow_com_(" + DataManager.Instance.GameData.ProgressData.CurrentKnifeFestLevel + ")");
        _winScreen.WinWithReward(multiplierLastStep);
        _isPlayerLose = false;
    }

    public void FailLevel()
    {
        _continueScreen.Enable();
        _isPlayerLose = true;
    }

    public void RestartSession()
    {
        GamemodManager.Instance.ControlSession(_isPlayerLose);
        PlayerInput.Instance.Enable();
        PlayerInput.Instance.AllowTap();
        _startScreen.Enable();
        IsSessionRestarted?.Invoke();
    }

    public void EndSession()
    {
        PlayerInput.Instance.DisallowUsing();
        GamemodManager.Instance.EndSession();
        PlayerInput.Instance.Enable();
        PlayerInput.Instance.DisallowTap();
        _startScreen.Enable();
        IsSessionRestarted?.Invoke();
    }

    private void OnScreenDisabled(bool isAdShowed)
    {
        DataManager.Instance.GameData.CanShowStartAd = isAdShowed;

        if (!isAdShowed)
        {
            EndSession();
            return;
        }

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
}
