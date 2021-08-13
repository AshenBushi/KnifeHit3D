using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OpenGift : MonoBehaviour
{
    private Button _button;

    public event UnityAction IsGiftOpened;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        AdManager.Instance.RewardedAd.OnAdFailedToLoad += HandleFailedToLoad;
        AdManager.Instance.RewardedAd.OnAdLoaded += HandleAdLoaded;
    }

    private void OnDisable()
    {
        AdManager.Instance.RewardedAd.OnAdFailedToLoad -= HandleFailedToLoad;
        AdManager.Instance.RewardedAd.OnAdLoaded -= HandleAdLoaded;
    }

    private void HandleAdLoaded(object sender, EventArgs e)
    {
        _button.interactable = true;
    }
    
    private void HandleFailedToLoad(object sender, AdFailedToLoadEventArgs adFailedToLoadEventArgs)
    {
        _button.interactable = false;
    }
    
    private void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        AdManager.Instance.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }

    private void HandleUserEarnReward(object sender, Reward e)
    {
        IsGiftOpened?.Invoke();
        
        AdManager.Instance.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }
    
    public void OpenGiftForAd()
    {
        MetricaManager.SendEvent("btn_open_gift");
        AdManager.Instance.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow += HandleFailedToShow;
        AdManager.Instance.ShowRewardVideo();
        _button.interactable = false;
    }
}
