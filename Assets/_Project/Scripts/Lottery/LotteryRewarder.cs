using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LotteryRewarder : MonoBehaviour
{
    [SerializeField] private Player _player;

    private bool _hasSkinReward = false;

    public void SendRewards(List<RewardName> rewards)
    {
        foreach (var reward in rewards)
        {
            switch (reward)
            {
                case RewardName.TenCoins:
                    MetricaManager.SendEvent("bns_lvl_10");
                    _player.DepositMoney(10);
                    break;
                case RewardName.TwentyCoins:
                    MetricaManager.SendEvent("bns_lvl_20");
                    _player.DepositMoney(20);
                    break;
                case RewardName.ThirtyCoins:
                    MetricaManager.SendEvent("bns_lvl_30");
                    _player.DepositMoney(30);
                    break;
                case RewardName.Slow:
                    MetricaManager.SendEvent("bns_lvl_slow");
                    DataManager.GameData.PlayerData.SlowMode++;
                    break;
                case RewardName.LevelPass:
                    MetricaManager.SendEvent("bns_lvl_skip");
                    DataManager.GameData.PlayerData.LevelPass++;
                    break;
                case RewardName.SecondChance:
                    MetricaManager.SendEvent("bns_lvl_chance");
                    DataManager.GameData.PlayerData.SecondLife++;
                    break;
                case RewardName.Skin:
                    MetricaManager.SendEvent("bns_lvl_skin");
                    RewardHandler.Instance.GiveLotteryReward();
                    _hasSkinReward = true;
                    break;
                case RewardName.Death:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!_hasSkinReward)
            {
                AsyncLoader.LoadScene();
            }
            
            DataManager.Save();
        }
    }
}
