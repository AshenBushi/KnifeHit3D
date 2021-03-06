using System;
using UnityEngine.Events;

public class LotteryTimer : Timer
{
    public event UnityAction IsTimeStart;
    public override event UnityAction IsTimeEnd;

    protected override void SaveTimer()
    {
        DataManager.Instance.GameData._lotteryClock = Clock;
        DataManager.Instance.SaveDate(DateTime.UtcNow);
        DataManager.Instance.Save();
    }

    protected override void LoadTimer()
    {
        if (DataManager.Instance.GameData.IsLotteryEnable)
        {
            Clock = new Clock();
            DisableTimer();
        }
        else
        {
            Clock = DataManager.Instance.GameData._lotteryClock;

            LastDate = DataManager.Instance.LoadDate();

            var secondsPassed = (int)(DateTime.UtcNow - LastDate).TotalSeconds;

            Clock.Seconds -= secondsPassed;

            CheckTimerForChanges();
        }
    }

    protected override void Countdown()
    {
        if (DataManager.Instance.GameData.IsLotteryEnable) return;
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
        DataManager.Instance.GameData.IsLotteryEnable = true;
        Clock.Hours = 0;
        Clock.Minutes = 0;
        Clock.Seconds = 0;
        
        ShowTime();
        
        IsTimeEnd?.Invoke();
    }

    public void EnableTimer()
    {
        DataManager.Instance.GameData.IsLotteryEnable = false;
        Clock.Hours = 0;
        Clock.Minutes = 4;
        Clock.Seconds = 59;
        LastDate = DateTime.UtcNow;
        
        ShowTime();
        
        IsTimeStart?.Invoke();
    }
}
