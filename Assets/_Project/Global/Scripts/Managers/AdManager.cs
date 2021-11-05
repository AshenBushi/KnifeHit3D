﻿using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.AdColony;
using GoogleMobileAds.Api.Mediation.AppLovin;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    private float _timeSpendFromLastInterstitial = 30f;

    public bool IsInterstitialShowed { get; private set; }
    public InterstitialAd Interstitial { get; private set; }
    public RewardedAd RewardedAd { get; private set; }
    public BannerView BannerAd { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        AppLovin.SetHasUserConsent(true);
        AppLovin.SetIsAgeRestrictedUser(true);
        AppLovin.Initialize();

        AdColonyAppOptions.SetGDPRRequired(true);
        AdColonyAppOptions.SetGDPRConsentString("1");

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
                        print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        });

        InitializeRewarded();
        InitializeInterstitial();
        InitializeBanner();
    }

    private void OnDisable()
    {
        Interstitial.OnAdClosed -= HandleOnAdClosed;
        Interstitial.OnAdFailedToShow -= HandleOnAdFailedToShow;
        RewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        RewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        RewardedAd.OnAdClosed -= HandleRewardedAdClosed;

        DataManager.Instance.GameData.CanShowStartAd = false;
        DataManager.Instance.Save();
    }

    private void Update()
    {
        if (_timeSpendFromLastInterstitial < 30)
            _timeSpendFromLastInterstitial += Time.deltaTime;
    }

    private void InitializeRewarded()
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

    private void InitializeInterstitial()
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
        Interstitial.OnAdFailedToShow += HandleOnAdFailedToShow;
        Interstitial.OnAdClosed += HandleOnAdClosed;
        Interstitial.LoadAd(request);
    }

    private void InitializeBanner()
    {
#if UNITY_ANDROID
        const string bannerId = "ca-app-pub-9672913692313370/4698557718";
#elif UNITY_IPHONE
        const string bannerId = "";
#else
        const string bannerId = "unexpected_platform";
#endif

        BannerAd = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        var request = new AdRequest.Builder().Build();

        BannerAd.LoadAd(request);
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        MetricaManager.SendEvent("int_show");
        InitializeInterstitial();
        IsInterstitialShowed = true;
    }

    private void HandleOnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        MetricaManager.SendEvent("int_fail");
        InitializeInterstitial();
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        MetricaManager.SendEvent("rew_fail");
        InitializeRewarded();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        MetricaManager.SendEvent("rew_show");
        InitializeRewarded();
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        InitializeRewarded();
    }

    public void ShowBanner()
    {
        BannerAd.Show();
    }

    public void HideBanner()
    {
        BannerAd.Hide();
    }

    public bool ShowInterstitial()
    {
        if (!Interstitial.IsLoaded() || _timeSpendFromLastInterstitial < 5f) return false;

        MetricaManager.SendEvent("int_start");
        Interstitial.Show();
        _timeSpendFromLastInterstitial = 0f;
        IsInterstitialShowed = false;
        return true;
    }

    public void ShowRewardVideo()
    {
        if (!RewardedAd.IsLoaded()) return;

        MetricaManager.SendEvent("rew_start");
        RewardedAd.Show();
    }
}
