using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[System.Serializable]
public struct MarkLevel
{
    public int Reward;
    public int KnifeReward;
    public List<MarkConfig> Marks;
}

[System.Serializable]
public struct CubeLevel
{
    public int Reward;
    public int KnifeReward;
    public TargetBase _base;
    public List<CubeConfig> Cubes;
}

[System.Serializable]
public struct FlatLevel
{
    public int Reward;
    public int KnifeReward;
    public List<FlatConfig> Flats;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<MarkLevel> _markLevels;
    [SerializeField] private List<CubeLevel> _cubeLevels;
    [SerializeField] private List<FlatLevel> _flatLevels;
    [SerializeField] private LevelColorPreset[] _colorPresets;

    private static int _markLevelCount;
    private static int _cubeLevelCount;
    private static int _flatLevelCount;
    private static List<MarkLevel> _markLvls;
    private static List<CubeLevel> _cubeLvls;
    private static List<FlatLevel> _flatLvls;
    private static LevelColorPreset[] _colorPrsts;
    private static int _presetIndex = 0;

    public static MarkLevel CurrentMarkLevel { get; private set; }
    public static CubeLevel CurrentCubeLevel { get; private set; }
    public static FlatLevel CurrentFlatLevel { get; private set; }

    public static event UnityAction<LevelColorPreset> IsColorChanged; 

    private void Awake()
    {
        _markLvls = _markLevels;
        _cubeLvls = _cubeLevels;
        _flatLvls = _flatLevels;
        _colorPrsts = _colorPresets;
        _markLevelCount = _markLevels.Count;
        _cubeLevelCount = _cubeLevels.Count;
        _flatLevelCount = _flatLevels.Count;
    }

    private void Start()
    {
        LoadLevel();
    }

    private static void LoadLevel()
    {
        CurrentMarkLevel = _markLvls[DataManager.GameData.ProgressData.CurrentMarkLevel];
        CurrentCubeLevel = _cubeLvls[DataManager.GameData.ProgressData.CurrentCubeLevel];
        CurrentFlatLevel = _flatLvls[DataManager.GameData.ProgressData.CurrentFlatLevel];
    }
    
    public static void NextMarkLevel()
    {
        if (DataManager.GameData.ProgressData.CurrentMarkLevel == _markLevelCount - 1) return;
        DataManager.GameData.ProgressData.CurrentMarkLevel++;
        DataManager.Save();
        LoadLevel();
    }
    
    public static void NextCubeLevel()
    {
        if (DataManager.GameData.ProgressData.CurrentCubeLevel == _cubeLevelCount - 1) return;
        DataManager.GameData.ProgressData.CurrentCubeLevel++;
        DataManager.Save();
        LoadLevel();
    }
    
    public static void NextFlatLevel()
    {
        if (DataManager.GameData.ProgressData.CurrentFlatLevel == _flatLevelCount - 1) return;
        DataManager.GameData.ProgressData.CurrentFlatLevel++;
        DataManager.Save();
        LoadLevel();
    }

    public static LevelColorPreset GiveColorPreset()
    {
        //var colorPreset = _colorPrsts[Random.Range(0, _colorPrsts.Length)];

        var colorPreset = _colorPrsts[_presetIndex];
        _presetIndex++;

        if (_presetIndex == _colorPrsts.Length)
            _presetIndex = 0;
        
        IsColorChanged?.Invoke(colorPreset);
        
        return colorPreset;
    }
}
