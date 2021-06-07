using System;
using UnityEngine;
using UnityEngine.Events;


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
            DataManager.GameData.ShopData.OpenedKnives.Add(i);
            DataManager.GameData.ShopData.CurrentKnifeIndex = i;
            DataManager.Save();
            _rewardScreen.Enable();
            break;
        }
    }

    public void GiveLevelCompleteReward(int knifeIndex)
    {
        DataManager.GameData.ShopData.OpenedKnives.Add(knifeIndex);
        DataManager.GameData.ShopData.CurrentKnifeIndex = knifeIndex;
        DataManager.Save();
        _rewardScreen.Enable();
    }
}
