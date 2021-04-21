using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Level
{
    public List<TargetConfig> Targets;
    public int Reward;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;

    private static int _levelCount;

    public static Level CurrentLevel { get; private set; }

    private void Awake()
    {
        LoadLevel();
        _levelCount = _levels.Count;
    }

    private void LoadLevel()
    {
        CurrentLevel = _levels[DataManager.GameData.Progress.CurrentLevel];
    }
    
    public static void NextLevel()
    {
        if (DataManager.GameData.Progress.CurrentLevel == _levelCount - 1) return;
        DataManager.GameData.Progress.CurrentLevel++;
        DataManager.Save();
    }
}
