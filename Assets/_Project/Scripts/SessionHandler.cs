using System;
using UnityEngine;
using UnityEngine.Events;

public class SessionHandler : Singleton<SessionHandler>
{
    [SerializeField] private LotteryTimer _lotteryTimer;
    [Header("Screens")]
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;

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

    private void OnSessionStart()
    {
        _startScreen.Disable();

        if (GamemodManager.Instance.LastPressedButtonIndex == -1)
            _lotteryTimer.EnableTimer();

        IsSessionStarted?.Invoke();
    }

    private void OnScreenDisabled(bool isAdShowed)
    {
        SceneLoader.Instance.PrepareScene(1);
        
        if(isAdShowed || !AdManager.Interstitial.IsLoaded())
        {
            RestartSession();
        }
        else
        {
            AdManager.Interstitial.OnAdClosed += HandleOnAdClosed;
            AdManager.ShowInterstitial();
        }
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        RestartSession();
    }

    public void CompleteLevel()
    {
        _winScreen.Win();
    }

    public void FailLevel()
    {
        _loseScreen.Lose();
    }

    private void RestartSession()
    {
        SceneLoader.Instance.LoadPreparedScene();
        /*GamemodManager.Instance.StartSession();
        _startScreen.Enable();
        IsSessionRestarted?.Invoke();*/
    }
}
