using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DoubleReward : AdButton
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _text;

    private int _chanceGiveX5;
    private int _coefficient;

    public event UnityAction IsWatchedReward;

    private void OnEnable()
    {
        _chanceGiveX5 = Random.Range(0, 100);

        _coefficient = _chanceGiveX5 < 20 ? 4 : 1;
        _text.text = "Watch X" + (1 + _coefficient).ToString();
    }

    protected override void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        MetricaManager.SendEvent("ev_rew_fail");
        base.HandleFailedToShow(sender, e);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        switch ((int)GamemodHandler.Instance.CurrentGamemod)
        {
            case 0 :
                _player.DepositMoney(LevelManager.CurrentMarkLevel.Reward * _coefficient);
                break;
            case 1 :
                _player.DepositMoney(LevelManager.CurrentCubeLevel.Reward * _coefficient);
                break;
            case 2 :
                _player.DepositMoney(LevelManager.CurrentFlatLevel.Reward * _coefficient);
                break;
            default:
                _player.DepositMoney(LevelManager.CurrentMarkLevel.Reward * _coefficient);
                break;
        }
        
        MetricaManager.SendEvent("ev_rew_show");
        IsWatchedReward?.Invoke();
        
        base.HandleUserEarnReward(sender, e);
    }
}
