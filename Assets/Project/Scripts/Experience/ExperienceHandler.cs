using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private RewardHandler _rewardHandler;
    [SerializeField] private SessionHandler _sessionHandler;
    [SerializeField] private GiftHandler _giftHandler;
    [SerializeField] private ExperienceBar _expBar;

    private bool _isLevelComplete;
    
    public bool HasReward { get; private set; } = false;
    
    private void CheckForFull()
    {
        if (DataManager.GameData.PlayerData.Experience < 200) return;
        DataManager.GameData.PlayerData.Experience -= 200;
        DataManager.Save();
        HasReward = true;
    }

    private void OnRewardGiven()
    {
        if (_giftHandler.HasGift)
        {
            _giftHandler.ShowGiftScreen(_isLevelComplete);
        }
        else
        {
            if (_isLevelComplete)
            {
                _sessionHandler.WinGame();
            }
            else
            {
                _sessionHandler.LoseGame();
            }
        }
    }
    
    public void GiveReward(bool isLevelCompleted)
    {
        for (var i = 29; i < 45; i++)
        {
            if (DataManager.GameData.ShopData.OpenedKnives.Contains(i)) continue;
            _isLevelComplete = isLevelCompleted;
            _rewardHandler.GiveGiftReward();
            _rewardHandler.IsRewardGiven += OnRewardGiven;
            break;
        }
    }

    public void AddExp(int value)
    {
        DataManager.GameData.PlayerData.Experience += value;
        DataManager.Save();
        CheckForFull();
        _expBar.ShowExpBar(value);
    }
}
