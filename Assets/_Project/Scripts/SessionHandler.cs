using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Handlers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SessionHandler : Singleton<SessionHandler>
{
    [Header("Screens")]
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;

    public event UnityAction IsSessionStarted;

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

        IsSessionStarted?.Invoke();
    }

    private void OnScreenDisabled(bool isAdShowed)
    {
        if(isAdShowed || !AdManager.Interstitial.IsLoaded())
        {
            AsyncLoader.PrepareScene();
            AsyncLoader.LoadScene();
        }
        else
        {
            AsyncLoader.PrepareScene();
            AdManager.Interstitial.OnAdClosed += HandleOnAdClosed;
            AdManager.ShowInterstitial();
        }
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        AsyncLoader.LoadScene();
    }

    public void CompleteLevel(int index = 0)
    {
        var rewardIndex = index;

        if (rewardIndex > 0)
        {
            RewardHandler.Instance.GiveLevelCompleteReward(rewardIndex);
        }
      
        _winScreen.Win();
    }

    public void FailLevel()
    {
        _loseScreen.Lose();
    }
}
