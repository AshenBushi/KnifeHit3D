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

        PlayerPrefs.SetInt("first_level", 1);
        StartCoroutine(SendMetricks());
    }

    private IEnumerator SendMetricks()
    {
        yield return new WaitForSeconds(.5f);

        MetricaManager.SendEvent("first_open");
    }

    public void Save()
    {
        File.WriteAllText(_path, JsonUtility.ToJson(GameData));
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
    public LotteryData LotteryData;
    public DisablingAdsData DisablingAds;
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
        ProgressData.CurrentFlatLevel = 0;
        ProgressData.CurrentMark2Level = 0;
        ProgressData.CurrentCube2Level = 0;
        ProgressData.CurrentFlat2Level = 0;

        SettingsData.SoundVolume = 1;
        SettingsData.MusicVolume = 1;

        DailyGiftsData._clock = new Clock() { Hours = 23, Minutes = 59, Seconds = 59 };
        DailyGiftsData.Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
        DailyGiftsData.UnlockedGifts = 1;
        DailyGiftsData.PickedGifts = 0;

        LotteryData.Clock = new Clock() { Hours = 0, Minutes = 4, Seconds = 59 };
        LotteryData.Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
        LotteryData.IsLotteryEnable = true;

        DisablingAds.Clock = new Clock() { Hours = 23, Minutes = 59, Seconds = 59 };
        DisablingAds.Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
        DisablingAds.CounterAdsOff = 0;
        DisablingAds.IsAdsDisableOneDay = false;

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
    public bool SecondLifeEnabled;
    public int Experience;
}

[Serializable]
public struct ProgressData
{
    public int CurrentGamemod;
    public int CurrentMarkLevel;
    public int CurrentCubeLevel;
    public int CurrentFlatLevel;
    public int CurrentMark2Level;
    public int CurrentCube2Level;
    public int CurrentFlat2Level;
    public int CurrentKnifeFestLevel;
    public int CurrentStackKnifeLevel;
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

    public void SaveData()
    {
        Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
    }

    public DateTime GetDate()
    {
        return Date != null ? DateTime.ParseExact(Date, "u", CultureInfo.InvariantCulture) : DateTime.UtcNow;
    }
}

[Serializable]
public struct LotteryData
{
    public Clock Clock;
    public string Date;
    public bool IsLotteryEnable;

    public void SaveData()
    {
        Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
    }

    public DateTime GetDate()
    {
        return Date != null ? DateTime.ParseExact(Date, "u", CultureInfo.InvariantCulture) : DateTime.UtcNow;
    }
}

[Serializable]
public struct DisablingAdsData
{
    public Clock Clock;
    public string Date;
    public int CounterAdsOff;
    public bool IsAdsDisableOneDay;

    public void SaveData()
    {
        Date = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
    }

    public DateTime GetDate()
    {
        return Date != null ? DateTime.ParseExact(Date, "u", CultureInfo.InvariantCulture) : DateTime.UtcNow;
    }
}
