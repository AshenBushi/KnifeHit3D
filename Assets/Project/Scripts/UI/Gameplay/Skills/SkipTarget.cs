using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class SkipTarget : AdButton
{
    [SerializeField] private TargetSpawner _targetSpawner;
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
        _targetSpawner.CurrentTarget.TryNextEdge();
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
