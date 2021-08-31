using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DailyGiftScreen : UIScreen
{
    [SerializeField] private Timer _timer;
    [SerializeField] private List<DailyGift> _gifts;
    [SerializeField] private StartScreen _startScreen;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _timer.IsTimeEnd += UnlockNextGift;

        foreach (var gift in _gifts)
        {
            gift.IsGotReward += GotReward;
        }
    }

    private void OnDisable()
    {
        _timer.IsTimeEnd -= UnlockNextGift;
        
        foreach (var gift in _gifts)
        {
            gift.IsGotReward -= GotReward;
        }
    }

    private void Start()
    {
        if (DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts != DataManager.Instance.GameData.DailyGiftsData.PickedGifts)
            Enable();
    }

    private void CheckGiftsState()
    {
        for (var i = 0; i < DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts; i++)
        {
            _gifts[i].Unlock();
        }
        
        for (var i = 0; i < DataManager.Instance.GameData.DailyGiftsData.PickedGifts; i++)
        {
            _gifts[i].Pick();
        }
    }
    
    private void UnlockNextGift()
    {
        if (_gifts.Count == DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts) return;
        _gifts[DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts].Unlock();
        DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts++;
        DataManager.Instance.Save();
        _startScreen.EnableGiftNotification();
        CheckGiftsState();
    }

    private void GotReward(bool isKnife, int value)
    {
        if (isKnife)
        {
            KnifeStorage.Instance.AddKnife(value);
        }
        else
        {
            Player.Instance.DepositMoney(value);
        }

        DataManager.Instance.GameData.DailyGiftsData.PickedGifts++;
        DataManager.Instance.Save();
        
        if(DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts == DataManager.Instance.GameData.DailyGiftsData.PickedGifts)
            _startScreen.DisableGiftNotification();
        CheckGiftsState();
        MetricaManager.SendEvent("day_gift");
    }
    
    public override void Enable()
    {
        base.Enable();
        CheckGiftsState();
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
    }

    public override void Disable()
    {
        base.Disable();
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
    }
}
