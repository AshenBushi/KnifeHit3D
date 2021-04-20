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
        GameData.PlayerData.Money = 0;

        GameData.Shop.CurrentKnifeIndex = 0;
        GameData.Shop.CurrentKnifePage = 0;
        GameData.Shop.OpenedKnives = new List<int> {0};

        GameData.Progress.CurrentLevel = 0;
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
    public Shop Shop;
    public PlayerData PlayerData;
    public Progress Progress;
}

[Serializable]
public struct Shop
{
    //Knife Data
    public int CurrentKnifeIndex;
    public int CurrentKnifePage;
    public List<int> OpenedKnives;
}

[Serializable]
public struct PlayerData
{
    public int Money;
}

[Serializable]
public struct Progress
{
    public int CurrentLevel;
}
