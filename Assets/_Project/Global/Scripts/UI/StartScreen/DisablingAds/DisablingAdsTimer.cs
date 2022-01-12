using System;
using UnityEngine.Events;

public class DisablingAdsTimer : Timer
{
    public event UnityAction IsTimeStart;
    public override event UnityAction IsTimeEnd;

    public void EnableTimer()
    {
        DataManager.Instance.GameData.DisablingAds.CounterAdsOff = 0;
        DataManager.Instance.GameData.DisablingAds.IsAdsDisableOneDay = true;
        DataManager.Instance.Save();
        Clock.Hours = 23;
        Clock.Minutes = 59;
        Clock.Seconds = 59;
        LastDate = DateTime.UtcNow;

        IsTimeStart?.Invoke();
    }

    private void DisableTimer()
    {
        DataManager.Instance.GameData.DisablingAds.IsAdsDisableOneDay = false;
        DataManager.Instance.Save();
        Clock.Hours = 0;
        Clock.Minutes = 0;
        Clock.Seconds = 0;

        IsTimeEnd?.Invoke();
    }

    protected override void LoadTimer()
    {
        if (!DataManager.Instance.GameData.DisablingAds.IsAdsDisableOneDay)
        {
            Clock = new Clock();
            DisableTimer();
        }
        else
        {
            Clock = DataManager.Instance.GameData.DisablingAds.Clock;

            LastDate = DataManager.Instance.GameData.DisablingAds.GetDate();

            var secondsPassed = (int)(DateTime.UtcNow - LastDate).TotalSeconds;

            Clock.Seconds -= secondsPassed;

            CheckTimerForChanges();
        }
    }

    protected override void SaveTimer()
    {
        DataManager.Instance.GameData.DisablingAds.Clock = Clock;
        DataManager.Instance.GameData.DisablingAds.SaveData();
        DataManager.Instance.Save();
    }

    protected override void CheckTimerForChanges()
    {
        while (Clock.Seconds < 0)
        {
            Clock.Seconds += 60;
            Clock.Minutes--;

            if (Clock.Minutes >= 0) continue;

            Clock.Minutes += 60;
            Clock.Hours--;

            if (Clock.Hours >= 0) continue;

            DisableTimer();
        }
    }

    protected override void Countdown()
    {
        if (!DataManager.Instance.GameData.DisablingAds.IsAdsDisableOneDay) return;

        base.Countdown();
    }

    protected override void ShowTime() { }
}
