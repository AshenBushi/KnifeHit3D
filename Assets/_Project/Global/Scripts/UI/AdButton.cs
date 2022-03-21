using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AdButton : MonoBehaviour
{
    protected Button _button;
    protected bool IsAdFailed = false;

    private void Awake()
    {
        _button = GetComponent<Button>();
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

    protected virtual void OnEnable()
    {
        AdManager.Instance.RewardedAd.OnAdFailedToLoad += HandleFailedToLoad;
        AdManager.Instance.RewardedAd.OnAdLoaded += HandleAdLoaded;
    }

    protected virtual void OnDisable()
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
