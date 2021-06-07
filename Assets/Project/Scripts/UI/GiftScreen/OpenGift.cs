using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class OpenGift : MonoBehaviour
{
    [SerializeField] private GiftScreen _giftScreen;
    [SerializeField] private RewardHandler _rewardHandler;
    
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

    private void OnRewardGiven()
    {
        _rewardHandler.IsRewardGiven -= OnRewardGiven;
        _giftScreen.Disable();
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
        _rewardHandler.GiveGiftReward();
        _rewardHandler.IsRewardGiven += OnRewardGiven;
        
        AdManager.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }
    
    public void OpenGiftForAd()
    {
        MetricaManager.SendEvent("btn_open_gift");
        AdManager.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow += HandleFailedToShow;
        AdManager.ShowRewardVideo();
        _button.interactable = false;
    }
}
