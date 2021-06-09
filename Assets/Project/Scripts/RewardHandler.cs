using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class RewardHandler : MonoBehaviour
{
    [SerializeField] private RewardScreen _rewardScreen;
    
    public event UnityAction IsRewardGiven;

    private void OnEnable()
    {
        _rewardScreen.IsScreenDisabled += OnScreenDisabled;
    }

    private void OnDisable()
    {
        _rewardScreen.IsScreenDisabled += OnScreenDisabled;
    }

    private void OnScreenDisabled()
    {
        IsRewardGiven?.Invoke();
    }

    public void GiveGiftReward()
    {
        for (var i = 29; i < 45; i++)
        {
            if (DataManager.GameData.ShopData.OpenedKnives.Contains(i)) continue;
            KnifeStorage.AddKnife(i);
            _rewardScreen.ShowReward(i);
            break;
        }
    }

    public void GiveLevelCompleteReward(int knifeIndex)
    {
        KnifeStorage.AddKnife(knifeIndex);
        _rewardScreen.ShowReward(knifeIndex);
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
        _rewardScreen.ShowReward(index);
    }
}
