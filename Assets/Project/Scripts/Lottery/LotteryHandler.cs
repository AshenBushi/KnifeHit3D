using System;
using System.Collections.Generic;
using Project.Scripts.Handlers;
using UnityEngine;

public class LotteryHandler : MonoBehaviour
{
    [SerializeField] private KnifeHandler _knifeHandler;
    [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
    [SerializeField] private LotterySpawner _lotterySpawner;
    [SerializeField] private LotteryScreen _lotteryScreen;

    private Lottery _lottery;
    private int _knifeAmount = 3;
    private int _maxRewardCount = 3;

    public List<RewardName> Rewards { get; private set; } = new List<RewardName>();

    private void OnDisable()
    {
        if(_lottery is null) return;
        _lottery.IsRewardTook -= OnRewardTook;
    }

    private void OnRewardTook(RewardName reward)
    {
        _hitScoreDisplayer.SubmitHit();
        
        Rewards.Add(reward);
        
        if (Rewards.Count >= _maxRewardCount)
        {
            EndLottery();
        }
    }
    
    private void EndLottery()
    {
        _lotteryScreen.Enable();
    }
    
    public void StartLottery()
    {
        _lottery = _lotterySpawner.SpawnLottery();
        _lottery.IsRewardTook += OnRewardTook;
        
        _knifeHandler.SetKnifeAmount(_lottery.HitToBreak);
        _knifeHandler.AllowThrow();
        _hitScoreDisplayer.SpawnHitScores(_lottery.HitToBreak);
    }
}
