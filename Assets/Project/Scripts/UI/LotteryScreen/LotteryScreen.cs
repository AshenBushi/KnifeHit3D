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
        CanvasGroup = GetComponent<CanvasGroup>();
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
        SceneManager.LoadScene(1);
    }
    
    public void SendReward(List<RewardNames> names)
    {
        Enable();

        for (var i = 0; i < names.Count; i++)
        {
            switch (names[i])
            {
                case RewardNames.TenCoins:
                    MetricaManager.SendEvent("bns_lvl_10");
                    _rewards[i].SetReward("10 coins");
                    _player.DepositMoney(10);
                    break;
                case RewardNames.TwentyCoins:
                    MetricaManager.SendEvent("bns_lvl_20");
                    _rewards[i].SetReward("20 coins");
                    _player.DepositMoney(20);
                    break;
                case RewardNames.ThirtyCoins:
                    MetricaManager.SendEvent("bns_lvl_30");
                    _rewards[i].SetReward("30 coins");
                    _player.DepositMoney(30);
                    break;
                case RewardNames.Slow:
                    MetricaManager.SendEvent("bns_lvl_slow");
                    _rewards[i].SetReward("Slow");
                    break;
                case RewardNames.LevelPass:
                    MetricaManager.SendEvent("bns_lvl_skip");
                    _rewards[i].SetReward("Level Pass");
                    break;
                case RewardNames.SecondChance:
                    MetricaManager.SendEvent("bns_lvl_chance");
                    _rewards[i].SetReward("Second Chance");
                    break;
                case RewardNames.Skin:
                    MetricaManager.SendEvent("bns_lvl_skin");
                    _rewards[i].SetReward("Skin");
                    OpenRandomSkin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

}
