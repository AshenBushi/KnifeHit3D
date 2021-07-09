using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static InterstitialAd Interstitial { get; private set; }
    public static RewardedAd RewardedAd{ get; private set; }

    private void Awake()
    {
        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        });
        
        InitializeRewarded();
        InitializeInterstitial();
    }

    private void OnDisable()
    {
        Interstitial.OnAdClosed -= HandleOnAdClosed;
        RewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        RewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        RewardedAd.OnAdClosed -= HandleRewardedAdClosed;
    }

    private static void InitializeRewarded()
    {
#if UNITY_ANDROID
        const string rewardId = "ca-app-pub-9672913692313370/5241874778"; 
#elif UNITY_IPHONE
        const string rewardId = "";
#else
        const string rewardId = "unexpected_platform";
#endif
        
        var request = new AdRequest.Builder().Build();
        
        RewardedAd = new RewardedAd(rewardId);
        RewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        RewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        RewardedAd.OnAdClosed += HandleRewardedAdClosed;
        RewardedAd.LoadAd(request);
    }

    private static void InitializeInterstitial()
    {
#if UNITY_ANDROID
        const string interstitialId = "ca-app-pub-9672913692313370/9922964234";
#elif UNITY_IPHONE
        const string interstitialId = "";
#else
        const string interstitialId = "unexpected_platform";
#endif
        
        var request = new AdRequest.Builder().Build();
        
        Interstitial = new InterstitialAd(interstitialId);
        Interstitial.OnAdClosed += HandleOnAdClosed;
        Interstitial.LoadAd(request);
    }
    
    private static void HandleOnAdClosed(object sender, EventArgs e)
    {
        InitializeInterstitial();
    }
    
    private static void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        InitializeRewarded();
    }
    
    private static void HandleUserEarnedReward(object sender, Reward e)
    {
        InitializeRewarded();
    }
    
    private static void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        InitializeRewarded();
    }

    public static void ShowInterstitial()
    {
        if (!Interstitial.IsLoaded()) return;
        MetricaManager.SendEvent("ad_int_start");
        Interstitial.Show();
    }

    public static void ShowRewardVideo()
    {
        if (!RewardedAd.IsLoaded()) return;
        MetricaManager.SendEvent("ad_rew_start");
        RewardedAd.Show();
    }
}
