using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private ExperienceBar _expBar;

    private void CheckForFull()
    {
        if (DataManager.GameData.PlayerData.Experience < 100) return;
        DataManager.GameData.PlayerData.Experience -= 100;
        DataManager.Save();
        GiveReward();
    }

    private void GiveReward()
    {
        for (var i = 29; i < 45; i++)
        {
            if (DataManager.GameData.ShopData.OpenedKnives.Contains(i)) continue;
            DataManager.GameData.ShopData.OpenedKnives.Add(i);
            break;
        }
    }
    
    public void AddExp(int value)
    {
        DataManager.GameData.PlayerData.Experience += value;
        DataManager.Save();
        CheckForFull();
        Debug.Log(DataManager.GameData.PlayerData.Experience);
        _expBar.ShowExpBar(value);
    }
}
