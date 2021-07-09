using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private ExperienceBar _expBar;

    public bool HasReward { get; private set; } = false;
    
    private void CheckForFull()
    {
        if (DataManager.GameData.PlayerData.Experience < 200) return;
        DataManager.GameData.PlayerData.Experience -= 200;
        DataManager.Save();
        HasReward = true;
    }

    public void AddExp(int value)
    {
        DataManager.GameData.PlayerData.Experience += value;
        DataManager.Save();
        CheckForFull();
        _expBar.ShowExpBar(value);
    }
}
