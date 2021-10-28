using System;
using UnityEngine.Events;

public class GiftTimer : Timer
{
    private bool isReady;
    public static UnityAction IsReady, IsNotReady;

    private void OnEnable()
    {
        IsReady += EnableReady;
        IsNotReady += DisableReady;
        LoadTimer();
    }

    private void OnDisable()
    {
        IsReady -= EnableReady;
        IsNotReady -= DisableReady;
        SaveTimer();
    }

    protected override void SaveTimer()
    {
        DataManager.Instance.GameData.DailyGiftsData._clock = Clock;
        DataManager.Instance.SaveDate(DateTime.UtcNow);
        DataManager.Instance.Save();
    }

    protected override void LoadTimer()
    {
        if (isReady)
        {
            Clock = new Clock();
            DisableTimer();
        }
        else
        {
            Clock = DataManager.Instance.GameData.DailyGiftsData._clock;

            LastDate = DataManager.Instance.LoadDate();

            var secondsPassed = (int)(DateTime.UtcNow - LastDate).TotalSeconds;

            Clock.Seconds -= secondsPassed;

            CheckTimerForChanges();
        }
    }

    private void DisableTimer()
    {
        isReady = true;
        Clock.Hours = 0;
        Clock.Minutes = 0;
        Clock.Seconds = 0;

        ShowTime();
        EnableReady();
    }

    public void EnableTimer()
    {
        isReady = false;
        Clock.Hours = 23;
        Clock.Minutes = 59;
        Clock.Seconds = 59;
        LastDate = DateTime.UtcNow;

        DisableReady();
        ShowTime();
    }

    private void EnableReady()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        _readyObject.SetActive(true);

    }

    private void DisableReady()
    {
        _readyObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
