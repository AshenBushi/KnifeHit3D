using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DailyGiftScreen : UIScreen
{
    [SerializeField] private Player _player;
    [SerializeField] private GiftTimer _giftTimer;
    [SerializeField] private List<DailyGift> _gifts;
    [SerializeField] private StartScreen _startScreen;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _giftTimer.CanGiveGift += UnlockNextGift;

        foreach (var gift in _gifts)
        {
            gift.IsGotReward += GotReward;
        }
    }

    private void OnDisable()
    {
        _giftTimer.CanGiveGift -= UnlockNextGift;
        
        foreach (var gift in _gifts)
        {
            gift.IsGotReward -= GotReward;
        }
    }

    private void Start()
    {
        if (DataManager.GameData.DailyGiftsData.UnlockedGifts != DataManager.GameData.DailyGiftsData.PickedGifts)
            Enable();
    }

    private void CheckGiftsState()
    {
        for (var i = 0; i < DataManager.GameData.DailyGiftsData.UnlockedGifts; i++)
        {
            _gifts[i].Unlock();
        }
        
        for (var i = 0; i < DataManager.GameData.DailyGiftsData.PickedGifts; i++)
        {
            _gifts[i].Pick();
        }
    }
    
    private void UnlockNextGift()
    {
        if (_gifts.Count == DataManager.GameData.DailyGiftsData.UnlockedGifts) return;
        _gifts[DataManager.GameData.DailyGiftsData.UnlockedGifts].Unlock();
        DataManager.GameData.DailyGiftsData.UnlockedGifts++;
        DataManager.Save();
        _startScreen.EnableGiftNotification();
        CheckGiftsState();
    }

    private void GotReward(bool isKnife, int value)
    {
        if (isKnife)
        {
            KnifeStorage.AddKnife(value);
        }
        else
        {
            _player.DepositMoney(value);
        }

        DataManager.GameData.DailyGiftsData.PickedGifts++;
        DataManager.Save();
        
        if(DataManager.GameData.DailyGiftsData.UnlockedGifts == DataManager.GameData.DailyGiftsData.PickedGifts)
            _startScreen.DisableGiftNotification();
        CheckGiftsState();
        MetricaManager.SendEvent("day_gift");
    }
    
    public override void Enable()
    {
        base.Enable();
        CheckGiftsState();
        SoundManager.PlaySound(SoundName.ButtonClick);
    }

    public override void Disable()
    {
        base.Disable();
        SoundManager.PlaySound(SoundName.ButtonClick);
    }
}
