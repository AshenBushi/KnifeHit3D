using System;
using System.Collections.Generic;
using Project.Scripts.Handlers;
using UnityEngine;

public class LotteryHandler : Singleton<LotteryHandler>
{
    [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
    [SerializeField] private LevelProgressDisplayer _levelProgressDisplayer;
    [SerializeField] private LotterySpawner _lotterySpawner;
    [SerializeField] private LotteryScreen _lotteryScreen;

    private Lottery _lottery;
    private int _knifeAmount = 3;
    private int _maxRewardCount = 3;

    public List<RewardName> Rewards { get; private set; } = new List<RewardName>();

    private void OnEnable()
    {
        GamemodHandler.Instance.IsModChanged += OnModChanged;
    }

    private void OnDisable()
    {
        GamemodHandler.Instance.IsModChanged -= OnModChanged;
        
        if(_lottery is null) return;
        _lottery.IsRewardTook -= OnRewardTook;
    }

    private void OnModChanged()
    {
        if(GamemodHandler.Instance.CurrentGamemod == GamemodName.Lottery)
        {
            StartLottery();
        }
        else
        {
            if(_lottery != null)
                Destroy(_lottery.gameObject);
        }
    }
    
    private void OnRewardTook(RewardName reward)
    {
        Rewards.Add(reward);

        if (reward == RewardName.Death)
        {
            KnifeHandler.Instance.DisallowThrow();
        }
        
        if (Rewards.Count >= _maxRewardCount)
        {
            EndLottery();
        }
    }
    
    private void EndLottery()
    {
        _lotteryScreen.Enable();
    }
    
    private void StartLottery()
    {
        _lottery = _lotterySpawner.SpawnLottery();
        _lottery.IsRewardTook += OnRewardTook;
        
        KnifeHandler.Instance.SetKnifeAmount(_lottery.HitToBreak);
        KnifeHandler.Instance.AllowThrow();
        _hitScoreDisplayer.SpawnHitScores(_lottery.HitToBreak);
        _levelProgressDisplayer.DisableDisplayer();
    }

    public void ContinuePlay()
    {
        _lotteryScreen.Disable();
        _lottery.AddHits();
        _maxRewardCount += _knifeAmount;
        KnifeHandler.Instance.SetKnifeAmount(_lottery.HitToBreak);
        _hitScoreDisplayer.SpawnHitScores(_lottery.HitToBreak);
    }
}
