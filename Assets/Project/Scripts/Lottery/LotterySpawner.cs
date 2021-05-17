using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LotterySpawner : MonoBehaviour
{
    [SerializeField] private Lottery _template;
    [SerializeField] private Player _player;
    [SerializeField] private LevelProgressDisplayer _levelProgressDisplayer;
    
    private Lottery _lottery;
    private int _hitCount = 3;

    public event UnityAction IsLose;
    public event UnityAction<List<RewardNames>> IsWin;
    public event UnityAction<int> IsLotterySpawned;

    private void OnDisable()
    {
        if (_lottery == null) return;
        
        _lottery.IsDeath -= OnDeath;
        _lottery.IsWin -= OnWin;
    }

    private void OnDeath()
    {
        MetricaManager.SendEvent("bns_lvl_death");
        IsLose?.Invoke();
    }

    private void OnWin(List<RewardNames> rewards)
    {
        IsWin?.Invoke(rewards);
    }

    public void ReplayLottery()
    {
        _lottery.AddMaxCount();
    }
    
    public void SpawnLottery()
    {
        _lottery = Instantiate(_template, transform);
        _lottery.IsDeath += OnDeath;
        _lottery.IsWin += OnWin;
        _levelProgressDisplayer.DisableDisplayer();
        _player.AllowThrow();
        IsLotterySpawned?.Invoke(_hitCount);
    }
}
