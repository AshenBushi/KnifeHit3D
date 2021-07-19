using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class RewardHandler : Singleton<RewardHandler>
{
    [SerializeField] private StartScreen _startScreen;

    public void GiveExperienceReward()
    {
        for (var i = 29; i < 45; i++)
        {
            if (DataManager.GameData.ShopData.OpenedKnives.Contains(i)) continue;
            KnifeStorage.AddKnife(i);
            break;
        }
        
        _startScreen.EnableShopNotification();
    }

    public void GiveLevelCompleteReward(int knifeIndex)
    {
        KnifeStorage.AddKnife(knifeIndex);
        
        _startScreen.EnableShopNotification();
    }

    public void GiveLotteryReward()
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
        
        KnifeStorage.AddKnife(index);
        
        _startScreen.EnableShopNotification();
    }
}
