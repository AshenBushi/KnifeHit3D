using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class SecondChance : MonoBehaviour
{
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private KnifeSpawner _knifeSpawner;

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
        _loseScreen.Disable();
        _knifeSpawner.SecondChance();
        
        AdManager.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }
    
    public void TakeSecondChance()
    {
        MetricaManager.SendEvent("btn_repeat");
        AdManager.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.RewardedAd.OnAdFailedToShow += HandleFailedToShow;
        AdManager.ShowRewardVideo();
        _button.interactable = false;
    }
}
