using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartScreen : UIScreen
{
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private GameObject _giftNotification;

    private GamemodHandler _gamemodHandler;

    public event UnityAction IsModChanged;

    private void Awake()
    {
        _gamemodHandler = GetComponentInChildren<GamemodHandler>();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _gamemodHandler.IsModChanged += OnModChanged;
        
        if(DataManager.GameData.DailyGiftsData.UnlockedGifts > DataManager.GameData.DailyGiftsData.PickedGifts)
            _giftNotification.SetActive(true);
    }

    private void OnDisable()
    {
        _gamemodHandler.IsModChanged -= OnModChanged;
    }

    private void OnModChanged()
    {
        IsModChanged?.Invoke();
    }

    public void EnableSettingsScreen()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        _settingsScreen.Enable();
    }
    
    public void EnableShopScreen()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        _shopScreen.Enable();
    }
    
    public void StartSession()
    {
        Disable();
    }
}
