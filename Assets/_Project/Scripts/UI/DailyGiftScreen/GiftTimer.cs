using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Timer
{
    public int Hours;
    public int Minutes;
    public int Seconds;
}

public class GiftTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _hours;
    [SerializeField] private TMP_Text _minutes;
    [SerializeField] private TMP_Text _seconds;

    private Timer _timer;
    private DateTime _lastDate;
    private float _timeSpend = 0f;

    public event UnityAction CanGiveGift;

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

    private void SaveTimer()
    {
        DataManager.GameData.DailyGiftsData.Timer = _timer;
        DataManager.SaveDate(DateTime.UtcNow);
        DataManager.Save();
    }

    private void LoadTimer()
    {
        _timer = DataManager.GameData.DailyGiftsData.Timer;
        _lastDate = DataManager.LoadDate();

        var secondsPassed = (int) (DateTime.UtcNow - _lastDate).TotalSeconds;

        _timer.Seconds -= secondsPassed;
        
        CheckTimerForChanges();
    }
    
    private void Countdown()
    {
        _timeSpend = (int) (DateTime.UtcNow - _lastDate).TotalSeconds;

        if (!(_timeSpend >= 1)) return;
        
        _timer.Seconds--;
        _lastDate = DateTime.UtcNow;
        CheckTimerForChanges();
        ShowTime();
    }

    private void ShowTime()
    {
        if (_timer.Hours >= 10)
            _hours.text = _timer.Hours.ToString();
        else
            _hours.text = "0" + _timer.Hours.ToString();

        if (_timer.Minutes >= 10)
            _minutes.text = _timer.Minutes.ToString();
        else
            _minutes.text = "0" + _timer.Minutes.ToString();
        
        if (_timer.Seconds >= 10)
            _seconds.text = _timer.Seconds.ToString();
        else
            _seconds.text = "0" + _timer.Seconds.ToString();
    }

    private void CheckTimerForChanges()
    {
        while (_timer.Seconds < 0)
        {
            _timer.Seconds += 60;
            _timer.Minutes--;

            if (_timer.Minutes >= 0) continue;
            _timer.Minutes += 60;
            _timer.Hours--;

            if (_timer.Hours >= 0) continue;
            _timer.Hours += 24;
            CanGiveGift?.Invoke();
        }
    }
}
