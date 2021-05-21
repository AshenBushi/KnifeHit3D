using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GiftHandler : MonoBehaviour
{
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private GiftScreen _giftScreen;
    [SerializeField] private Gift _giftTemplate;

    private GiftSpawner _giftSpawner;
    private bool _isPlayerWin;
    public bool HasGift { get; private set; }

    public event UnityAction<bool, float> IsGiftScreenDisabled;

    private void OnEnable()
    {
        _targetSpawner.IsLevelSpawned += InitializeGift;
        _giftScreen.IsGiftScreenDisable += OnGiftScreenDisable;
    }

    private void OnDisable()
    {
        _targetSpawner.IsLevelSpawned -= InitializeGift;
        _giftScreen.IsGiftScreenDisable += OnGiftScreenDisable;
        
        if(_giftSpawner != null)
            _giftSpawner.IsGiftSliced -= OnGiftSliced;
    }

    private void InitializeGift(GiftSpawner spawner)
    {
        _giftSpawner = spawner;

        _giftSpawner.SpawnGift(_giftTemplate);

        _giftSpawner.IsGiftSliced += OnGiftSliced;
    }
    
    private void OnGiftScreenDisable()
    {
        IsGiftScreenDisabled?.Invoke(_isPlayerWin, 0f);
    }
    
    private void OnGiftSliced()
    {
        HasGift = true;
    }

    public void ShowGiftScreen(bool isPlayerWin)
    {
        _isPlayerWin = isPlayerWin;
        StartCoroutine(_giftScreen.GiftScreenAnimation());
    }
    
    public void GiveGift()
    {
        for (var i = 29; i < 45; i++)
        {
            if (DataManager.GameData.ShopData.OpenedKnives.Contains(i)) continue;
            DataManager.GameData.ShopData.OpenedKnives.Add(i);
            DataManager.GameData.ShopData.CurrentKnifeIndex = i;
            break;
        }
    }
}
