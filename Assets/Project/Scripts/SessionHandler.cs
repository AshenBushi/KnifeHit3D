﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Handlers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SessionHandler : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [Header("Handlers")]
    [SerializeField] private RewardHandler _rewardHandler;
    [SerializeField] private TargetHandler _targetHandler;
    [SerializeField] private KnifeHandler _knifeHandler;
    [SerializeField] private LotteryHandler _lotteryHandler;
    [SerializeField] private AppleHandler _appleHandler;
    [SerializeField] private GiftHandler _giftHandler;
    [SerializeField] private ExperienceHandler _experienceHandler;
    [Header("Screens")]
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;

    private int Gamemod => DataManager.GameData.ProgressData.CurrentGamemod;
    
    public event UnityAction IsSessionStarted;
    public event UnityAction IsLotteryStarted;

    private void OnEnable()
    {
        _inputField.IsSessionStart += OnSessionStart;
        _targetHandler.IsLevelComplete += OnLevelComplete;
        _knifeHandler.IsLevelFailed += OnLevelFailed;
        _winScreen.IsScreenDisabled += OnScreenDisabled;
        _loseScreen.IsScreenDisabled += OnScreenDisabled;
    }

    private void OnDisable()
    {
        _inputField.IsSessionStart -= OnSessionStart;
        _targetHandler.IsLevelComplete -= OnLevelComplete;
        _knifeHandler.IsLevelFailed -= OnLevelFailed;
        _winScreen.IsScreenDisabled -= OnScreenDisabled;
        _loseScreen.IsScreenDisabled -= OnScreenDisabled;
    }

    private void OnSessionStart()
    {
        _startScreen.Disable();

        switch (Gamemod)
        {
            case 0:
                MetricaManager.SendEvent("target_lvl_start_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                break;
            case 1:
                MetricaManager.SendEvent("cube_lvl_start_(" + DataManager.GameData.ProgressData.CurrentCubeLevel + ")");
                break;
            case 2:
                MetricaManager.SendEvent("flat_lvl_start_(" + DataManager.GameData.ProgressData.CurrentFlatLevel + ")");
                break;
        }
        
        IsSessionStarted?.Invoke();
    }
    
    private void OnLevelComplete()
    {
        StartCoroutine(EnableEndScreen(true));
    }
    
    private void OnLevelFailed()
    {
        StartCoroutine(EnableEndScreen(false));
    }

    private void OnScreenDisabled(bool isAdShowed)
    {
        if (_appleHandler.SlicedAppleCount >= 3)
        {
            _targetHandler.ClearTargets();
            _lotteryHandler.StartLottery();
            _appleHandler.DisableCounter();
            IsLotteryStarted?.Invoke();
        }
        else
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
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        AsyncLoader.LoadScene();
    }

    private void OnRewardGiven()
    {
        _rewardHandler.IsRewardGiven -= OnRewardGiven;
        _winScreen.Win();
    }

    private IEnumerator EnableEndScreen(bool isLevelComplete)
    {
        _knifeHandler.DisallowThrow();
        
        yield return new WaitForSeconds(1f);
        
        if(_experienceHandler.HasReward)
        {
            _experienceHandler.GiveReward(isLevelComplete);
        }
        else
        {
            if (_giftHandler.HasGift)
            {
                _giftHandler.ShowGiftScreen(isLevelComplete);
            }
            else
            {
                if (isLevelComplete)
                {
                    WinGame();
                }
                else
                {
                    LoseGame();
                }
            }
        }
    }
    
    public void WinGame()
    {
        var rewardIndex = 0;

        switch (Gamemod)
        {
            case 0:
                MetricaManager.SendEvent("target_lvl_complete_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                rewardIndex = LevelManager.CurrentMarkLevel.KnifeReward;
                LevelManager.NextMarkLevel();
                break;
            case 1:
                MetricaManager.SendEvent("cube_lvl_start_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                rewardIndex = LevelManager.CurrentCubeLevel.KnifeReward;
                LevelManager.NextCubeLevel();
                break;
            case 2:
                MetricaManager.SendEvent("flat_lvl_complete_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                rewardIndex = LevelManager.CurrentFlatLevel.KnifeReward;
                LevelManager.NextFlatLevel();
                break;
        }

        if (rewardIndex > 0)
        {
            _rewardHandler.GiveLevelCompleteReward(rewardIndex);
            _rewardHandler.IsRewardGiven += OnRewardGiven;
        }
        else
        {
            _winScreen.Win();
        }
    }

    public void LoseGame()
    {
        switch (Gamemod)
        {
            case 0:
                MetricaManager.SendEvent("target_lvl_fail_(" + DataManager.GameData.ProgressData.CurrentMarkLevel + ")");
                break;
            case 1:
                MetricaManager.SendEvent("cube_lvl_fail_(" + DataManager.GameData.ProgressData.CurrentCubeLevel + ")");
                break;
            case 2:
                MetricaManager.SendEvent("flat_lvl_fail_(" + DataManager.GameData.ProgressData.CurrentFlatLevel + ")");
                break;
        }
        
        _loseScreen.Lose();
    }
}