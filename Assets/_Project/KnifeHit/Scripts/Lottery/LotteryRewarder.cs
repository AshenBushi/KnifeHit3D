using System;
using System.Collections.Generic;
using UnityEngine;

public class LotteryRewarder : MonoBehaviour
{
    private bool _hasSkinReward = false;

    public void SendRewards(List<RewardName> rewards)
    {
        foreach (var reward in rewards)
        {
            switch (reward)
            {
                case RewardName.TenCoins:
                    Player.Instance.DepositMoney(10);
                    break;
                case RewardName.TwentyCoins:
                    Player.Instance.DepositMoney(20);
                    break;
                case RewardName.ThirtyCoins:
                    Player.Instance.DepositMoney(30);
                    break;
                case RewardName.Slow:
                    DataManager.Instance.GameData.PlayerData.SlowMode++;
                    break;
                case RewardName.LevelPass:
                    DataManager.Instance.GameData.PlayerData.LevelPass++;
                    break;
                case RewardName.SecondChance:
                    DataManager.Instance.GameData.PlayerData.SecondLife++;
                    break;
                case RewardName.Skin:
                    RewardHandler.Instance.GiveLotteryReward();
                    _hasSkinReward = true;
                    break;
                case RewardName.Death:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        
        DataManager.Instance.Save();
        SessionHandler.Instance.AllowPlayerLose();
        SessionHandler.Instance.EndSession();
    }
}
