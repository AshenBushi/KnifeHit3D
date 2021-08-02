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
            if (DataManager.Instance.GameData.ShopData.OpenedKnives.Contains(i)) continue;
            KnifeStorage.Instance.AddKnife(i);
            break;
        }
        
        _startScreen.EnableShopNotification();
    }

    public void GiveLevelCompleteReward(int knifeIndex)
    {
        KnifeStorage.Instance.AddKnife(knifeIndex);
        
        _startScreen.EnableShopNotification();
    }

    public void GiveLotteryReward()
    {
        var index = Random.Range(45, 63);
        var lockedKnifeCount = 0;
        
        for (var i = 45; i < 63; i++)
        {
            if (!DataManager.Instance.GameData.ShopData.OpenedKnives.Contains(i))
            {
                lockedKnifeCount++;
            }
        }

        if (lockedKnifeCount == 0)
        {
            return;
        }

        while (DataManager.Instance.GameData.ShopData.OpenedKnives.Contains(index))
        {
            index = Random.Range(45, 63);
        }
        
        KnifeStorage.Instance.AddKnife(index);
        
        _startScreen.EnableShopNotification();
    }
}
