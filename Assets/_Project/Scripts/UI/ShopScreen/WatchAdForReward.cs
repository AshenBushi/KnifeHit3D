using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
public class WatchAdForReward : AdButton
{
    [SerializeField] private Player _player;
    
    private int _moneyReward;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _player.DepositMoney(_moneyReward);
        
        base.HandleUserEarnReward(sender, e);
    }
    
    public void WatchAd(int value)
    {
        _moneyReward = value;
        
        base.WatchAd();
        
        Button.interactable = true;
    }
}
