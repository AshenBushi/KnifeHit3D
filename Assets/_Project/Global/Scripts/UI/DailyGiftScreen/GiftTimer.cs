using System;

public class GiftTimer : Timer
{
    protected override void SaveTimer()
    {
        DataManager.Instance.GameData.DailyGiftsData._clock = Clock;
        DataManager.Instance.SaveDate(DateTime.UtcNow);
        DataManager.Instance.Save();
    }

    protected override void LoadTimer()
    {
        Clock = DataManager.Instance.GameData.DailyGiftsData._clock;

        base.LoadTimer();
    }
}
