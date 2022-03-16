using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AdButton : MonoBehaviour
{
    protected Button Button;
    protected bool IsAdFailed = false;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void Update()
    {
        if (!AdManager.Instance.RewardedAd.IsLoaded())
        {
            IsAdFailed = true;
            //Button.interactable = false;
        }
        else
        {
            IsAdFailed = false;
            //Button.interactable = true;
        }
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
        Button.interactable = true;
    }

    private void HandleFailedToLoad(object sender, AdFailedToLoadEventArgs adFailedToLoadEventArgs)
    {
        Button.interactable = false;
    }

    protected virtual void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        AdManager.Instance.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }

    protected virtual void HandleUserEarnReward(object sender, Reward e)
    {
        AdManager.Instance.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }

    public virtual void WatchAd()
    {
        if (IsAdFailed)
        {
            NotificationScreen.Instance.SetNotify("Подождите, реклама загружается");
            return;
        }

        AdManager.Instance.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow += HandleFailedToShow;

        AdManager.Instance.ShowRewardVideo();

        //Button.interactable = false;
    }

}
