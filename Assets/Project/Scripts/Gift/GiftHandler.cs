using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Handlers;
using UnityEngine;
using UnityEngine.Events;

public class GiftHandler : MonoBehaviour
{
    [SerializeField] private SessionHandler _sessionHandler;
    [SerializeField] private TargetHandler _targetHandler;
    [SerializeField] private GiftScreen _giftScreen;
    [SerializeField] private int _indexWhereSpawn;

    private List<GiftSpawner> _giftSpawners = new List<GiftSpawner>();
    private Gift _gift;
    private bool _isLevelComplete;

    private int Gamemod => DataManager.GameData.ProgressData.CurrentGamemod;
    public bool HasGift { get; private set; }

    private void OnEnable()
    {
        _targetHandler.IsLevelSpawned += OnLevelSpawned;
        _giftScreen.IsScreenDisabled += OnScreenDisabled;
    }

    private void OnDisable()
    {
        _targetHandler.IsLevelSpawned -= OnLevelSpawned;
        _giftScreen.IsScreenDisabled += OnScreenDisabled;

        if(_gift != null)
            _gift.IsSliced -= OnGiftSliced;
    }

    private void OnLevelSpawned()
    {
        _giftSpawners.Clear();
        
        if(Gamemod != 1)
        {
            foreach (var target in _targetHandler.Targets)
            {
                _giftSpawners.Add(target.GetComponentInChildren<GiftSpawner>());
            }
        }
        else
        {
            _giftSpawners = _targetHandler.Targets[0].GetComponentsInChildren<GiftSpawner>().ToList();
        }

        SpawnGift();
    }
    
    private void OnScreenDisabled()
    {
        HasGift = false;
        StartCoroutine(_sessionHandler.EnableEndScreen(_isLevelComplete));
    }

    private void SpawnGift()
    {
        _gift = _giftSpawners[_indexWhereSpawn].SpawnGift();
    }

    private void OnGiftSliced()
    {
        HasGift = true;
    }

    public void ShowGiftScreen(bool isLevelComplete)
    {
        _isLevelComplete = isLevelComplete;
        StartCoroutine(_giftScreen.EnableAnimation());
    }
}
