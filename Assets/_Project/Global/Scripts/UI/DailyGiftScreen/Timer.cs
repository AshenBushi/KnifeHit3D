using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Clock
{
    public int Hours;
    public int Minutes;
    public int Seconds;
}

public class Timer : MonoBehaviour
{
    [SerializeField] protected GameObject _readyObject;
    [SerializeField] private TMP_Text _hours;
    [SerializeField] private TMP_Text _minutes;
    [SerializeField] private TMP_Text _seconds;

    [SerializeField] protected Clock _timeToCountdown;

    private float _timeSpend = 0f;

    protected DateTime LastDate;
    protected Clock Clock;

    public virtual event UnityAction IsTimeEnd;

    private void OnEnable()
    {
        LoadTimer();
    }

    private void OnDisable()
    {
        SaveTimer();
    }

    private void Update()
    {
        Countdown();
    }

    protected virtual void Countdown()
    {
        _timeSpend = (int)(DateTime.UtcNow - LastDate).TotalSeconds;

        if (!(_timeSpend >= 1)) return;

        Clock.Seconds--;
        LastDate = DateTime.UtcNow;
        CheckTimerForChanges();
        ShowTime();
    }

    protected virtual void ShowTime()
    {
        if (Clock.Hours >= 10)
            _hours.text = Clock.Hours.ToString();
        else
            _hours.text = "0" + Clock.Hours;

        if (Clock.Minutes >= 10)
            _minutes.text = Clock.Minutes.ToString();
        else
            _minutes.text = "0" + Clock.Minutes;

        if (Clock.Seconds >= 10)
            _seconds.text = Clock.Seconds.ToString();
        else
            _seconds.text = "0" + Clock.Seconds;
    }

    protected virtual void CheckTimerForChanges()
    {
        while (Clock.Seconds < 0)
        {
            Clock.Seconds += _timeToCountdown.Seconds;
            Clock.Minutes--;

            if (Clock.Minutes >= 0) continue;
            Clock.Minutes += _timeToCountdown.Minutes;
            Clock.Hours--;

            if (Clock.Hours >= 0) continue;
            Clock.Hours += _timeToCountdown.Hours;
            IsTimeEnd?.Invoke();
        }
    }

    protected virtual void SaveTimer()
    { }

    protected virtual void LoadTimer()
    {
        LastDate = DataManager.Instance.GameData.DailyGiftsData.GetDate();

        var secondsPassed = (int)(DateTime.UtcNow - LastDate).TotalSeconds;

        Clock.Seconds -= secondsPassed;

        CheckTimerForChanges();
    }
}
