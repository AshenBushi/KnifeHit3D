using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class RewardHandler : Singleton<RewardHandler>
{
    [SerializeField] private StartScreen _startScreen;

    private void GiveFirstKnifeInRange(int startIndex, int endIndex)
    {
        for (var i = startIndex; i <= endIndex; i++)
        {
            if (DataManager.Instance.GameData.ShopData.OpenedKnives.Contains(i)) continue;
            KnifeStorage.Instance.AddKnife(i);
            _startScreen.EnableShopNotification();
            break;
        }
    }
    
    public void GiveExperienceReward()
    {
        GiveFirstKnifeInRange(18, 35);
    }

    public void GiveLevelCompleteReward()
    {
        GiveFirstKnifeInRange(0, 17);
    }

    public void GiveLotteryReward()
    {
        GiveFirstKnifeInRange(36, 53);
    }
}
