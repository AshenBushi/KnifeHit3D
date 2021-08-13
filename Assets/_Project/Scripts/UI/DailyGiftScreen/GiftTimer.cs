using System;

public class GiftTimer : Timer
{
    protected override void SaveTimer()
    {
        DataManager.Instance.GameData.DailyGiftsData.Time = Time;
        DataManager.Instance.SaveDate(DateTime.UtcNow);
        DataManager.Instance.Save();
    }

    protected override void LoadTimer()
    {
        Time = DataManager.Instance.GameData.DailyGiftsData.Time;

        base.LoadTimer();
    }
}
