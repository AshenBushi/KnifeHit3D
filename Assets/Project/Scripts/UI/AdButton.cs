using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdButton : MonoBehaviour
{
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
    
    protected virtual void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        AdManager.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }

    protected virtual void HandleUserEarnReward(object sender, Reward e)
    {
        AdManager.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }
    
    public virtual void WatchAd()
    {
        AdManager.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow += HandleFailedToShow;
        AdManager.ShowRewardVideo();
        _button.interactable = false;
    }
    
}
