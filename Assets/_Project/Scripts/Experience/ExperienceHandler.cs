using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceHandler : Singleton<ExperienceHandler>
{
    [SerializeField] private ExperienceBar _expBar;

    public bool HasReward { get; private set; } = false;
    
    private void CheckForFull()
    {
        if (DataManager.Instance.GameData.PlayerData.Experience < 200) return;
        DataManager.Instance.GameData.PlayerData.Experience -= 200;
        DataManager.Instance.Save();
        RewardHandler.Instance.GiveExperienceReward();
    }

    public void AddExp(int value)
    {
        DataManager.Instance.GameData.PlayerData.Experience += value;
        DataManager.Instance.Save();
        CheckForFull();
        _expBar.ShowExpBar(value);
    }
}
