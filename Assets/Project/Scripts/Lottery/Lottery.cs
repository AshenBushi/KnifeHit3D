using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum RewardNames
{
    TenCoins = 0,
    TwentyCoins,
    ThirtyCoins,
    Slow,
    LevelPass,
    SecondChance,
    Skin,
    Death
}

public class Lottery : MonoBehaviour
{
    [SerializeField] private List<RotateDefinition> _definitions;
    
    private List<LotterySection> _sections;
    private Tween _rotator;
    private List<RewardNames> _rewards = new List<RewardNames>();
    private int _maxRewardCount = 3;

    public event UnityAction IsDeath;
    public event UnityAction<List<RewardNames>> IsWin;
    
    private void Awake()
    {
        _sections = GetComponentsInChildren<LotterySection>().ToList();
        Rotate();
    }

    private void OnEnable()
    {
        foreach (var section in _sections)
        {
            section.IsKnifeStuck += OnKnifeStuck;
        }
    }

    private void OnDisable()
    {
        foreach (var section in _sections)
        {
            section.IsKnifeStuck -= OnKnifeStuck;
        }
    }

    private void Rotate()
    {
        var currentDefinition = _definitions[Random.Range(0, _definitions.Count)];
        var rotateEuler = new Vector3(0f, 0f, currentDefinition.Angle);

        _rotator = transform.DORotate(transform.eulerAngles + rotateEuler, currentDefinition.Duration, RotateMode.FastBeyond360)
            .SetEase(currentDefinition.EaseCurve).SetLink(gameObject);
        
        _rotator.OnComplete(() =>
        {
            Rotate();
        });
    }

    private void OnKnifeStuck(RewardNames reward)
    {
        _rewards.Add(reward);

        if (reward == RewardNames.Death)
        {
            IsDeath?.Invoke();
            return;
        }
        
        if (_rewards.Count >= _maxRewardCount)
        {
            IsWin?.Invoke(_rewards);
        }
    }
    
    public void AddMaxCount()
    {
        _maxRewardCount = 6;
    }
}
