using System;
using UnityEngine.Events;

public class LotteryTimer : Timer
{
    public event UnityAction IsTimeStart;
    public override event UnityAction IsTimeEnd;

    protected override void SaveTimer()
    {
        DataManager.Instance.GameData.LotteryTime = Time;
        DataManager.Instance.SaveDate(DateTime.UtcNow);
        DataManager.Instance.Save();
    }

    protected override void LoadTimer()
    {
        if (DataManager.Instance.GameData.IsLotteryEnable)
        {
            Time = new Time();
            DisableTimer();
        }
        else
        {
            Time = DataManager.Instance.GameData.LotteryTime;

            LastDate = DataManager.Instance.LoadDate();

            var secondsPassed = (int)(DateTime.UtcNow - LastDate).TotalSeconds;

            Time.Seconds -= secondsPassed;

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
        while (Time.Seconds < 0)
        {
            Time.Seconds += 60;
            Time.Minutes--;

            if (Time.Minutes >= 0) continue;
            DisableTimer();
        }
    }

    private void DisableTimer()
    {
        DataManager.Instance.GameData.IsLotteryEnable = true;
        Time.Hours = 0;
        Time.Minutes = 0;
        Time.Seconds = 0;
        
        ShowTime();
        
        IsTimeEnd?.Invoke();
    }

    public void EnableTimer()
    {
        DataManager.Instance.GameData.IsLotteryEnable = false;
        Time.Hours = 0;
        Time.Minutes = 4;
        Time.Seconds = 59;
        LastDate = DateTime.UtcNow;
        
        ShowTime();
        
        IsTimeStart?.Invoke();
    }
}
