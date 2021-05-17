using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePlayLottery : MonoBehaviour
{
    [SerializeField] private LotteryScreen _lotteryScreen;
    [SerializeField] private KnifeSpawner _knifeSpawner;
    [SerializeField] private LotterySpawner _lotterySpawner;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        AdManager.RewardedAd.OnAdFailedToLoad += HandleFailedToLoad;
        AdManager.RewardedAd.OnAdLoaded += HandleAdLoaded;
    }

    private void OnDisable()
    {
        AdManager.RewardedAd.OnAdFailedToLoad -= HandleFailedToLoad;
        AdManager.RewardedAd.OnAdLoaded -= HandleAdLoaded;
    }

    private void HandleAdLoaded(object sender, EventArgs e)
    {
        _button.interactable = true;
    }
    
    private void HandleFailedToLoad(object sender, AdErrorEventArgs e)
    {
        _button.interactable = false;
    }
    
    private void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        AdManager.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }

    private void HandleUserEarnReward(object sender, Reward e)
    {
        _lotteryScreen.Disable();
        _lotterySpawner.ReplayLottery();
        _knifeSpawner.ReplayLottery();
        
        AdManager.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }
    
    public void ReplayLottery()
    {
        MetricaManager.SendEvent("btn_repeat");
        AdManager.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow += HandleFailedToShow;
        AdManager.ShowRewardVideo();
        _button.interactable = false;
    }
}
