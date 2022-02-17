using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MarkLevel
{
    public int Reward;
    public bool KnifeReward;
    public List<MarkConfig> Marks;
}

[System.Serializable]
public struct CubeLevel
{
    public int Reward;
    public bool KnifeReward;
    public TargetBase _base;
    public List<CubeConfig> Cubes;
}

[System.Serializable]
public struct FlatLevel
{
    public int Reward;
    public bool KnifeReward;
    public List<FlatConfig> Flats;
}

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<MarkLevel> _markLevels;
    [SerializeField] private List<CubeLevel> _cubeLevels;
    [SerializeField] private List<FlatLevel> _flatLevels;

    public MarkLevel CurrentMarkLevel { get; private set; }
    public MarkLevel CurrentMark2Level { get; private set; }

    public CubeLevel CurrentCubeLevel { get; private set; }
    public CubeLevel CurrentCube2Level { get; private set; }

    public FlatLevel CurrentFlatLevel { get; private set; }
    public FlatLevel CurrentFlat2Level { get; private set; }

    private void Start()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        CurrentMarkLevel = _markLevels[DataManager.Instance.GameData.ProgressData.CurrentMarkLevel];
        CurrentCubeLevel = _cubeLevels[DataManager.Instance.GameData.ProgressData.CurrentCubeLevel];
        CurrentFlatLevel = _flatLevels[DataManager.Instance.GameData.ProgressData.CurrentFlatLevel];
        CurrentMark2Level = _markLevels[DataManager.Instance.GameData.ProgressData.CurrentMark2Level];
        CurrentCube2Level = _cubeLevels[DataManager.Instance.GameData.ProgressData.CurrentCube2Level];
        CurrentFlat2Level = _flatLevels[DataManager.Instance.GameData.ProgressData.CurrentFlat2Level];
    }

    public void NextMarkLevel()
    {
        if (DataManager.Instance.GameData.ProgressData.CurrentMarkLevel == _markLevels.Count - 1) return;
        DataManager.Instance.GameData.ProgressData.CurrentMarkLevel++;
        DataManager.Instance.Save();
        LoadLevel();
    }

    public void NextCubeLevel()
    {
        if (DataManager.Instance.GameData.ProgressData.CurrentCubeLevel == _cubeLevels.Count - 1) return;
        DataManager.Instance.GameData.ProgressData.CurrentCubeLevel++;
        DataManager.Instance.Save();
        LoadLevel();
    }

    public void NextFlatLevel()
    {
        if (DataManager.Instance.GameData.ProgressData.CurrentFlatLevel == _flatLevels.Count - 1) return;
        DataManager.Instance.GameData.ProgressData.CurrentFlatLevel++;
        DataManager.Instance.Save();
        LoadLevel();
    }

    public void NextMark2Level()
    {
        if (DataManager.Instance.GameData.ProgressData.CurrentMark2Level == _markLevels.Count - 1) return;
        DataManager.Instance.GameData.ProgressData.CurrentMark2Level++;
        DataManager.Instance.Save();
        LoadLevel();
    }

    public void NextCube2Level()
    {
        if (DataManager.Instance.GameData.ProgressData.CurrentCube2Level == _cubeLevels.Count - 1) return;
        DataManager.Instance.GameData.ProgressData.CurrentCube2Level++;
        DataManager.Instance.Save();
        LoadLevel();
    }

    public void NextFlat2Level()
    {
        if (DataManager.Instance.GameData.ProgressData.CurrentFlat2Level == _flatLevels.Count - 1) return;
        DataManager.Instance.GameData.ProgressData.CurrentFlat2Level++;
        DataManager.Instance.Save();
        LoadLevel();
    }

    public void NextKnifeFestLevel()
    {
        DataManager.Instance.GameData.ProgressData.CurrentKnifeFestLevel++;
        DataManager.Instance.Save();
        LoadLevel();
    }

    public void NextStackKnifeLevel()
    {
        DataManager.Instance.GameData.ProgressData.CurrentStackKnifeLevel++;
        DataManager.Instance.Save();
        LoadLevel();
    }
}
