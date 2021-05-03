using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TargetLevel
{
    public int Reward;
    public List<TargetConfig> Targets;
}

[System.Serializable]
public struct CubeLevel
{
    public int Reward;
    public TargetBase _base;
    public List<CubeConfig> Cubes;
}

[System.Serializable]
public struct FlatLevel
{
    public int Reward;
    public List<FlatConfig> Flats;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<TargetLevel> _targetLevels;
    [SerializeField] private List<CubeLevel> _cubeLevels;
    [SerializeField] private List<FlatLevel> _flatLevels;

    private static int _targetLevelCount;
    private static int _cubeLevelCount;
    private static int _flatLevelCount;

    public static TargetLevel CurrentTargetLevel { get; private set; }
    public static CubeLevel CurrentCubeLevel { get; private set; }
    public static FlatLevel CurrentFlatLevel { get; private set; }

    private void Awake()
    {
        _targetLevelCount = _targetLevels.Count;
        _cubeLevelCount = _cubeLevels.Count;
        _flatLevelCount = _flatLevels.Count;
    }

    private void OnEnable()
    {
        LoadLevel();
    }

    private void Start()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        CurrentTargetLevel = _targetLevels[DataManager.GameData.ProgressData.CurrentTargetLevel];
        CurrentCubeLevel = _cubeLevels[DataManager.GameData.ProgressData.CurrentCubeLevel];
        CurrentFlatLevel = _flatLevels[DataManager.GameData.ProgressData.CurrentFlatLevel];
    }
    
    public static void NextTargetLevel()
    {
        if (DataManager.GameData.ProgressData.CurrentTargetLevel == _targetLevelCount - 1) return;
        DataManager.GameData.ProgressData.CurrentTargetLevel++;
        DataManager.Save();
    }
    
    public static void NextCubeLevel()
    {
        if (DataManager.GameData.ProgressData.CurrentCubeLevel == _cubeLevelCount - 1) return;
        DataManager.GameData.ProgressData.CurrentCubeLevel++;
        DataManager.Save();
    }
    
    public static void NextFlatLevel()
    {
        if (DataManager.GameData.ProgressData.CurrentFlatLevel == _flatLevelCount - 1) return;
        DataManager.GameData.ProgressData.CurrentFlatLevel++;
        DataManager.Save();
    }
}
