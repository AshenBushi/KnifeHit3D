using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static InterstitialAd Interstitial { get; private set; }
    public static RewardedAd RewardedAd{ get; private set; }

    private void Awake()
    {
        MobileAds.Initialize(initStatus => { });
        InitializeAds();
    }

    private static void InitializeAds()
    {
#if UNITY_ANDROID
        const string interstitialId = "ca-app-pub-9672913692313370/9922964234";
        const string rewardId = "ca-app-pub-9672913692313370/5241874778"; 
#elif UNITY_IPHONE
        const string interstitialId = "";
        const string rewardId = "";
#else
        const string interstitialId = "unexpected_platform";
        const string rewardId = "unexpected_platform";
#endif
        
        Interstitial = new InterstitialAd(interstitialId);
        RewardedAd = new RewardedAd(rewardId);
        var request = new AdRequest.Builder().Build();
        Interstitial.LoadAd(request);
        request = new AdRequest.Builder().Build();
        RewardedAd.LoadAd(request);
    }

    public static void ShowInterstitial()
    {
        if (!Interstitial.IsLoaded()) return;
        MetricaManager.SendEvent("ad_int_start");
        Interstitial.Show();
        InitializeAds();
    }

    public static void ShowRewardVideo()
    {
        if (!RewardedAd.IsLoaded()) return;
        MetricaManager.SendEvent("ad_rew_start");
        RewardedAd.Show();
        InitializeAds();
    }
}
