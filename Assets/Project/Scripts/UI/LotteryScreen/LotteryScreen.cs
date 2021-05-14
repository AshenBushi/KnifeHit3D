using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LotteryScreen : UIScreen
{
    [SerializeField] private Player _player;

    private List<LotteryReward> _rewards;

    private void Awake()
    {
        _rewards = GetComponentsInChildren<LotteryReward>().ToList();
    }

    private void OpenRandomSkin()
    {
        var index = Random.Range(45, 63);
        var lockedKnifeCount = 0;
        
        for (var i = 45; i < 63; i++)
        {
            if (!DataManager.GameData.ShopData.OpenedKnives.Contains(i))
            {
                lockedKnifeCount++;
            }
        }

        if (lockedKnifeCount == 0)
        {
            return;
        }

        while (DataManager.GameData.ShopData.OpenedKnives.Contains(index))
        {
            index = Random.Range(45, 63);
        }
        
        DataManager.GameData.ShopData.OpenedKnives.Add(index);
        DataManager.GameData.ShopData.CurrentKnifeIndex = index;
    }
    
    
    public void Collect()
    {
        SceneManager.LoadScene(0);
    }
    
    public void SendReward(List<RewardNames> names)
    {
        Enable();

        for (var i = 0; i < names.Count; i++)
        {
            switch (names[i])
            {
                case RewardNames.TenCoins:
                    _rewards[i].SetReward("10 coins");
                    _player.DepositMoney(10);
                    break;
                case RewardNames.TwentyCoins:
                    _rewards[i].SetReward("20 coins");
                    _player.DepositMoney(20);
                    break;
                case RewardNames.ThirtyCoins:
                    _rewards[i].SetReward("30 coins");
                    _player.DepositMoney(30);
                    break;
                case RewardNames.Slow:
                    _rewards[i].SetReward("Slow");
                    break;
                case RewardNames.LevelPass:
                    _rewards[i].SetReward("Level Pass");
                    break;
                case RewardNames.SecondChance:
                    _rewards[i].SetReward("Second Chance");
                    break;
                case RewardNames.Skin:
                    _rewards[i].SetReward("Skin");
                    OpenRandomSkin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

}
