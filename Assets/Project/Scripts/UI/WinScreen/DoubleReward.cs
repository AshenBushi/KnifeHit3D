using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DoubleReward : AdButton
{
    [SerializeField] private Player _player;

    public event UnityAction IsWatchedReward;

    protected override void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        MetricaManager.SendEvent("ev_rew_fail");
        base.HandleFailedToShow(sender, e);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0 :
                _player.DepositMoney(LevelManager.CurrentTargetLevel.Reward);
                break;
            case 1 :
                _player.DepositMoney(LevelManager.CurrentCubeLevel.Reward);
                break;
            case 2 :
                _player.DepositMoney(LevelManager.CurrentFlatLevel.Reward);
                break;
            default:
                _player.DepositMoney(LevelManager.CurrentTargetLevel.Reward);
                break;
        }
        
        MetricaManager.SendEvent("ev_rew_show");
        IsWatchedReward?.Invoke();
        
        base.HandleUserEarnReward(sender, e);
    }
}
