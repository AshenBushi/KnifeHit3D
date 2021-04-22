using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Level
{
    public int Reward;
    public List<TargetConfig> Targets;
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
        CurrentLevel = _levels[DataManager.GameData._progressData.CurrentLevel];
    }
    
    public static void NextLevel()
    {
        if (DataManager.GameData._progressData.CurrentLevel == _levelCount - 1) return;
        DataManager.GameData._progressData.CurrentLevel++;
        DataManager.Save();
    }
}
