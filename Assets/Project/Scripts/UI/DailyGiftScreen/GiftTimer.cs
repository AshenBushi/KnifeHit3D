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

    private void Start()
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
        ShowTime();
    }

    private void SaveTimer()
    {
        DataManager.GameData.DailyGiftsData.Timer = _timer;
        DataManager.SaveDate(_lastDate);
        DataManager.Save();
    }

    private void LoadTimer()
    {
        _timer = DataManager.GameData.DailyGiftsData.Timer;
        _lastDate = DateTime.UtcNow;

        var secondsPassed = (int) (_lastDate - DataManager.LoadDate()).TotalSeconds;

        _timer.Hours -= secondsPassed / 3600;

        while (_timer.Hours < 0)
        {
            _timer.Hours += 24;
            CanGiveGift?.Invoke();
        }

        secondsPassed %= 3600;

        _timer.Minutes -= secondsPassed / 60;

        if (_timer.Minutes < 0)
        {
            _timer.Minutes += 60;
            _timer.Hours--;

            if (_timer.Hours < 0)
            {
                _timer.Hours = 23;
            }
        }
        
        _timer.Seconds -= secondsPassed % 60;

        if (_timer.Seconds < 0)
        {
            _timer.Seconds += 60;
            _timer.Minutes--;

            if (_timer.Minutes < 0)
            {
                _timer.Minutes = 59;
            }
        }
    }
    
    private void Countdown()
    {
        if (_timer.Hours == 0 && _timer.Minutes == 0 && _timer.Seconds == 0)
        {
            _timer.Hours = 24;
            _timer.Minutes = 0;
            _timer.Seconds = 0;
            CanGiveGift?.Invoke();
            Debug.Log("Work1");
        }
        
        if (_timer.Minutes == 0 && _timer.Seconds == 0)
        {
            _timer.Hours--;
            _timer.Minutes = 60;
            _timer.Seconds = 0;
        }
        
        if (_timer.Seconds == 0)
        {
            _timer.Minutes--;
            _timer.Seconds = 59;
        }

        _timeSpend = (int) (DateTime.UtcNow - _lastDate).TotalSeconds;

        if (!(_timeSpend >= 1)) return;
        _timer.Seconds--;
        
        _lastDate = DateTime.UtcNow;
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
}
