using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Project.Scripts.Handlers;
using UnityEngine;
using UnityEngine.UI;

public class SkipTarget : AdButton
{
    [SerializeField] private Image _adIcon;

    private void OnEnable()
    {
        if (DataManager.Instance.GameData.PlayerData.LevelPass <= 0)
        {
            _adIcon.gameObject.SetActive(true);
        }
    }

    private void Skip()
    {
        DataManager.Instance.GameData.PlayerData.LevelPass--;
        DataManager.Instance.Save();
        TargetHandler.Instance.CurrentTarget.BreakTarget();
        gameObject.SetActive(false);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        Skip();

        base.HandleUserEarnReward(sender, e);
    }

    public void ActivateSkip()
    {
        if (DataManager.Instance.GameData.PlayerData.LevelPass > 0)
        {
            Skip();
        }
        else
        {
            WatchAd();
        }
    }
}
