using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static string _path;

    public static GameData GameData = new GameData();

    private static bool _dataIsLoad = false;

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _path = Path.Combine(Application.persistentDataPath, "GameData.json");
#else
        _path = Path.Combine(Application.dataPath, "GameData.json");
#endif

        if (File.Exists(_path))
        {
            GameData = JsonUtility.FromJson<GameData>(File.ReadAllText(_path));
        }
        else
        {
            FirstPlay();
            File.WriteAllText(_path, JsonUtility.ToJson(GameData));
        }

        _dataIsLoad = true;
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        Save();
    }
#endif
    private void OnApplicationQuit()
    {
        Save();
    }
    
    private void FirstPlay()
    {
        GameData.PlayerData.Money = 5000;

        GameData.ShopData.CurrentKnifeIndex = 0;
        GameData.ShopData.OpenedKnives = new List<int>
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
            29, 30, 31, 32, 33, 34, 35, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44
        };

        GameData.ProgressData.CurrentTargetLevel = 0;
        GameData.ProgressData.CurrentCubeLevel = 0;
        GameData.ProgressData.CurrentCubeLevel = 0;
        GameData.ProgressData.CurrentGamemod = 0;
    }
    
    public static void Save()
    {
        File.WriteAllText(_path, JsonUtility.ToJson(GameData));
    }

    public static bool IsLoaded()
    {
        return _dataIsLoad;
    }
}

[Serializable]

public class GameData
{
    public ShopData ShopData;
    public PlayerData PlayerData;
    public ProgressData ProgressData;
}

[Serializable]
public struct ShopData
{
    public int CurrentKnifeIndex;
    public List<int> OpenedKnives;
}

[Serializable]
public struct PlayerData
{
    public int Money;
}

[Serializable]
public struct ProgressData
{
    public int CurrentGamemod;
    public int CurrentTargetLevel;
    public int CurrentCubeLevel;
    public int CurrentFlatLevel;
}
