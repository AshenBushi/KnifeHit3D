using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private string _path;

    public GameData GameData;

    protected override void Awake()
    {
        base.Awake();
        
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
            Save();
        }
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
        GameData = new GameData();
        StartCoroutine(SendMetricks());
    }

    private IEnumerator SendMetricks()
    {
        yield return new WaitForSeconds(.5f);
        
        MetricaManager.SendEvent("ev_first_open");
    }
    
    public void Save()
    {
        File.WriteAllText(_path, JsonUtility.ToJson(GameData));
    }
    
    public void SaveDate(DateTime value)
    {
        var convert = value.ToString("u", CultureInfo.InvariantCulture);
        GameData.DailyGiftsData.Date = convert;
    }

    public DateTime LoadDate()
    {
        var result = GameData.DailyGiftsData.Date != null ? DateTime.ParseExact(GameData.DailyGiftsData.Date, "u", CultureInfo.InvariantCulture) : DateTime.UtcNow;

        return result;
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
    public Clock _lotteryClock;
    public bool IsLotteryEnable;
    public Gamemod CurrentGamemod;
    public bool CanShowStartAd;

    public GameData()
    {
        PlayerData.Money = 0;

        ShopData.CurrentKnifeIndex = 0;
        ShopData.OpenedKnives = new List<int>
        {
            0
        };

        ProgressData.CurrentMarkLevel = 0;
        ProgressData.CurrentCubeLevel = 0;
        ProgressData.CurrentCubeLevel = 0;

        SettingsData.SoundVolume = 1;
        SettingsData.MusicVolume = 1;

        DailyGiftsData._clock = new Clock() {Hours = 23, Minutes =59, Seconds = 59};
        DailyGiftsData.Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
        DailyGiftsData.UnlockedGifts = 1;
        DailyGiftsData.PickedGifts = 0;

        _lotteryClock = new Clock() { Hours = 0, Minutes = 4, Seconds = 59 };
        IsLotteryEnable = true;
        CanShowStartAd = false;
    }
    
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
    public int CurrentMarkLevel;
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
    public Clock _clock;
    public string Date;
    public int UnlockedGifts;
    public int PickedGifts;
}
