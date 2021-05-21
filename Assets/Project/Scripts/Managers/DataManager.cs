using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

        GameData.ShopData.CurrentKnifeIndex = 0;
        GameData.ShopData.OpenedKnives = new List<int>
        {
            0
        };

        GameData.ProgressData.CurrentTargetLevel = 0;
        GameData.ProgressData.CurrentCubeLevel = 0;
        GameData.ProgressData.CurrentCubeLevel = 0;
        GameData.ProgressData.CurrentGamemod = 0;

        GameData.SettingsData.SoundVolume = 1;
        GameData.SettingsData.MusicVolume = 1;

        GameData.DailyGiftsData.Timer = new Timer() {Hours = 23, Minutes =59, Seconds = 59};
        GameData.DailyGiftsData.Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
        GameData.DailyGiftsData.UnlockedGifts = 1;
        GameData.DailyGiftsData.PickedGifts = 0;

        StartCoroutine(SendMetricks());
    }

    private IEnumerator SendMetricks()
    {
        yield return new WaitForSeconds(.5f);
        
        MetricaManager.SendEvent("ev_first_open");
    }
    
    public static void Save()
    {
        File.WriteAllText(_path, JsonUtility.ToJson(GameData));
    }
    
    public static void SaveDate(DateTime value)
    {
        var convert = value.ToString("u", CultureInfo.InvariantCulture);
        GameData.DailyGiftsData.Date = convert;
    }

    public static DateTime LoadDate()
    {
        var result = GameData.DailyGiftsData.Date != null ? DateTime.ParseExact(GameData.DailyGiftsData.Date, "u", CultureInfo.InvariantCulture) : DateTime.UtcNow;

        return result;
    }

    public static bool Loaded()
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
    public SettingsData SettingsData;
    public DailyGiftsData DailyGiftsData;
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
    public int SlowMode;
    public int LevelPass;
    public int SecondLife;
    public int Experience;
}

[Serializable]
public struct ProgressData
{
    public int CurrentGamemod;
    public int CurrentTargetLevel;
    public int CurrentCubeLevel;
    public int CurrentFlatLevel;
}

[Serializable]
public struct SettingsData
{
    public float SoundVolume;
    public float MusicVolume;
}

[Serializable]
public struct DailyGiftsData
{
    public Timer Timer;
    public string Date;
    public int UnlockedGifts;
    public int PickedGifts;
}
