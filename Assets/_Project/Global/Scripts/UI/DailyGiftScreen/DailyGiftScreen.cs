using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyGiftScreen : UIScreen
{
    [SerializeField] private Timer _timer;
    [SerializeField] private List<DailyGift> _gifts;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private DailyGiftArrows _dailyArrows;
    [SerializeField] private ScrollRect _scrollRect;

    private int _nextGiftIndex;

    public static UnityAction IsScrollDisable, IsScrollEnable;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        _nextGiftIndex = DataManager.Instance.GameData.DailyGiftsData.PickedGifts;
    }

    private void OnEnable()
    {
        IsScrollDisable += ScrollDisable;
        IsScrollEnable += ScrollEnable;

        _timer.IsTimeEnd += UnlockNextGift;

        foreach (var gift in _gifts)
        {
            gift.IsGotReward += GotReward;
            gift.DailyArrows = _dailyArrows;
        }

        if (_nextGiftIndex < _gifts.Count)
            _gifts[_nextGiftIndex].SetPositionArrows(false);
        else
            _gifts[DataManager.Instance.GameData.DailyGiftsData.PickedGifts].SetPositionArrows(false);
    }

    private void OnDisable()
    {
        IsScrollDisable -= ScrollDisable;
        IsScrollEnable -= ScrollEnable;

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

        if (DataManager.Instance.GameData.DailyGiftsData.PickedGifts < _gifts.Count - 1)
        {
            DataManager.Instance.GameData.DailyGiftsData.PickedGifts++;
            _nextGiftIndex++;
            _gifts[_nextGiftIndex].SetPositionArrows(true);
        }
        else if (DataManager.Instance.GameData.DailyGiftsData.PickedGifts == _gifts.Count - 1)
        {
            DataManager.Instance.GameData.DailyGiftsData.PickedGifts++;
        }
        DataManager.Instance.Save();
        if (DataManager.Instance.GameData.DailyGiftsData.PickedGifts == DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts)
            _startScreen.DisableGiftNotification();



        CheckGiftsState();
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
        base.Enable();
        CheckGiftsState();
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
    }

    public override void Disable()
    {
        base.Disable();
        gameObject.SetActive(false);
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
    }

    private void ScrollDisable()
    {
        _scrollRect.vertical = false;
    }

    private void ScrollEnable()
    {
        _scrollRect.vertical = true;
    }
}
