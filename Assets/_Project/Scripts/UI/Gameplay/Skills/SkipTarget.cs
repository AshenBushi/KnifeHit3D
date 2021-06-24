using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Project.Scripts.Handlers;
using UnityEngine;
using UnityEngine.UI;

public class SkipTarget : AdButton
{
    [SerializeField] private TargetHandler _targetHandler;
    [SerializeField] private Image _adIcon;

    private void Start()
    {
        if (DataManager.GameData.PlayerData.LevelPass <= 0)
        {
            _adIcon.gameObject.SetActive(true);
        }
    }

    private void Skip()
    {
        DataManager.GameData.PlayerData.LevelPass--;
        DataManager.Save();
        _targetHandler.CurrentTarget.BreakTarget();
        gameObject.SetActive(false);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        Skip();

        base.HandleUserEarnReward(sender, e);
    }

    public void ActivateSkip()
    {
        if (DataManager.GameData.PlayerData.LevelPass > 0)
        {
            Skip();
        }
        else
        {
            WatchAd();
        }
    }
}
