using System;
using System.Collections.Generic;
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
        
        TryToShowAd();
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
        DataManager.Instance.GameData.CanShowStartAd = !isAdShowed;
        RestartSession();
    }

    private void TryToShowAd()
    {
        if(DataManager.Instance.GameData.CanShowStartAd)
        {
            AdManager.Instance.ShowInterstitial();
        }
    }

    public void CompleteLevel()
    {
        _winScreen.Win();
        SceneLoader.Instance.PrepareScene(1);
    }

    public void FailLevel()
    {
        _loseScreen.Lose();
        SceneLoader.Instance.PrepareScene(1);
    }

    private void RestartSession()
    {
        SceneLoader.Instance.LoadPreparedScene();
        /*GamemodManager.Instance.StartSession();
        _startScreen.Enable();
        IsSessionRestarted?.Invoke();*/
    }
}
