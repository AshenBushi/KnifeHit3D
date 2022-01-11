using System;
using UnityEngine.Events;

public class LotteryTimer : Timer
{
    public event UnityAction IsTimeStart;
    public override event UnityAction IsTimeEnd;

    protected override void SaveTimer()
    {
        DataManager.Instance.GameData.LotteryData.Clock = Clock;
        DataManager.Instance.GameData.LotteryData.SaveData();
        DataManager.Instance.Save();
    }

    protected override void LoadTimer()
    {
        if (DataManager.Instance.GameData.LotteryData.IsLotteryEnable)
        {
            Clock = new Clock();
            DisableTimer();
        }
        else
        {
            Clock = DataManager.Instance.GameData.LotteryData.Clock;

            LastDate = DataManager.Instance.GameData.LotteryData.GetDate();

            var secondsPassed = (int)(DateTime.UtcNow - LastDate).TotalSeconds;

            Clock.Seconds -= secondsPassed;

            CheckTimerForChanges();
        }
    }

    protected override void Countdown()
    {
        if (DataManager.Instance.GameData.LotteryData.IsLotteryEnable) return;
        base.Countdown();
    }

    protected override void CheckTimerForChanges()
    {
        while (Clock.Seconds < 0)
        {
            Clock.Seconds += 60;
            Clock.Minutes--;

            if (Clock.Minutes >= 0) continue;
            DisableTimer();
        }
    }

    private void DisableTimer()
    {
        DataManager.Instance.GameData.LotteryData.IsLotteryEnable = true;
        DataManager.Instance.Save();
        Clock.Hours = 0;
        Clock.Minutes = 0;
        Clock.Seconds = 0;

        ShowTime();

        EnableReady();

        IsTimeEnd?.Invoke();
    }

    public void EnableTimer()
    {
        DataManager.Instance.GameData.LotteryData.IsLotteryEnable = false;
        DataManager.Instance.Save();
        Clock.Hours = 0;
        Clock.Minutes = 4;
        Clock.Seconds = 59;
        LastDate = DateTime.UtcNow;

        DisableReady();

        ShowTime();

        IsTimeStart?.Invoke();
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
