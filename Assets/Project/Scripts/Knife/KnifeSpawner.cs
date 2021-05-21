using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeSpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private LotterySpawner _lotterySpawner;
    [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
    [SerializeField] private List<Knife> _knives;
    
    private Knife _currentKnife;
    private int _knifeAmount = 0;
    private bool _isSecondLife = false;
    
    public Knife CurrentTemplate => _knives[DataManager.GameData.ShopData.CurrentKnifeIndex];

    public event UnityAction<bool> IsLose;

    private void OnEnable()
    {
        _targetSpawner.IsNewTargetSet += OnNewTargetSet;
        _lotterySpawner.IsLotterySpawned += OnNewTargetSet;
    }

    private void OnDisable()
    {
        _targetSpawner.IsNewTargetSet -= OnNewTargetSet;
        _lotterySpawner.IsLotterySpawned -= OnNewTargetSet;
        if (_currentKnife == null) return;
        _currentKnife.IsStuck -= SpawnKnife;
        _currentKnife.IsBounced -= OnKnifeBounced;
    }
    
    private void OnKnifeBounced()
    {
        Time.timeScale = 1;
        _player.DisallowThrow();
        SpawnKnife();

        if (DataManager.GameData.PlayerData.SecondLife > 0 && !_isSecondLife)
        {
            _player.AllowThrow();
            _knifeAmount++;
            DataManager.GameData.PlayerData.SecondLife--;
            DataManager.Save();
            _isSecondLife = true;
        }
        else
        {
            IsLose?.Invoke(false);
        }
    }

    private void OnNewTargetSet(int amount)
    {
        _knifeAmount = amount;
        _hitScoreDisplayer.SpawnHitScores(amount);
    }

    private void OnKnifeStuck()
    {
        _hitScoreDisplayer.SubmitHit();
        SpawnKnife();
    }
    
    public void SpawnKnife()
    {
        if (_currentKnife != null) return;
        _currentKnife = Instantiate(_knives[DataManager.GameData.ShopData.CurrentKnifeIndex], _player.transform);
        _currentKnife.IsStuck += OnKnifeStuck;
        _currentKnife.IsBounced += OnKnifeBounced;
    }

    public void ThrowKnife()
    {
        if (_currentKnife == null)
        {
            SpawnKnife();
        }

        _knifeAmount--;
        _currentKnife.Throw();
        _currentKnife = null;

        if (_knifeAmount < 1)
        {
            _player.DisallowThrow();
        }
    }

    public void ReplayLottery()
    {
        _player.AllowThrow();
        _knifeAmount = 3;
        _hitScoreDisplayer.SpawnHitScores(3);
    }
    
    public void SecondChance()
    {
        _knifeAmount += 3;
        _player.AllowThrow();
    }
    
    public void Reload()
    {
        _currentKnife.IsStuck -= OnKnifeStuck;
        _currentKnife.IsBounced -= OnKnifeBounced;
        Destroy(_currentKnife.gameObject);
        _currentKnife = null;
        SpawnKnife();
    }
}
