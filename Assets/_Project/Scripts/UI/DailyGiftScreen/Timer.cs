using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Time
{
    public int Hours;
    public int Minutes;
    public int Seconds;
}

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _hours;
    [SerializeField] private TMP_Text _minutes;
    [SerializeField] private TMP_Text _seconds;
    [SerializeField] private Time _timeToCountdown;
    
    private float _timeSpend = 0f;
    
    protected DateTime LastDate;
    protected Time Time;
    
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
        _timeSpend = (int) (DateTime.UtcNow - LastDate).TotalSeconds;

        if (!(_timeSpend >= 1)) return;
        
        Time.Seconds--;
        LastDate = DateTime.UtcNow;
        CheckTimerForChanges();
        ShowTime();
    }

    protected void ShowTime()
    {
        if (Time.Hours >= 10)
            _hours.text = Time.Hours.ToString();
        else
            _hours.text = "0" + Time.Hours;

        if (Time.Minutes >= 10)
            _minutes.text = Time.Minutes.ToString();
        else
            _minutes.text = "0" + Time.Minutes;
        
        if (Time.Seconds >= 10)
            _seconds.text = Time.Seconds.ToString();
        else
            _seconds.text = "0" + Time.Seconds;
    }

    protected virtual void CheckTimerForChanges()
    {
        while (Time.Seconds < 0)
        {
            Time.Seconds += _timeToCountdown.Seconds;
            Time.Minutes--;

            if (Time.Minutes >= 0) continue;
            Time.Minutes += _timeToCountdown.Minutes;
            Time.Hours--;

            if (Time.Hours >= 0) continue;
            Time.Hours += _timeToCountdown.Hours;
            IsTimeEnd?.Invoke();
        }
    }
    
    protected virtual void SaveTimer()
    { }

    protected virtual void LoadTimer()
    {
        LastDate = DataManager.Instance.LoadDate();
        
        var secondsPassed = (int) (DateTime.UtcNow - LastDate).TotalSeconds;

        Time.Seconds -= secondsPassed;
        
        CheckTimerForChanges();
    }
}
