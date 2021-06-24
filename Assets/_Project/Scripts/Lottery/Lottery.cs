using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum RewardName
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

    public int HitToBreak = 3;
    
    public event UnityAction<RewardName> IsRewardTook;

    private void Awake()
    {
        _sections = GetComponentsInChildren<LotterySection>().ToList();
        Rotate();
    }

    private void OnEnable()
    {
        foreach (var section in _sections)
        {
            section.IsRewardTook += OnRewardTook;
        }
    }

    private void OnDisable()
    {
        foreach (var section in _sections)
        {
            section.IsRewardTook -= OnRewardTook;
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

    private void OnRewardTook(RewardName reward)
    {
        IsRewardTook?.Invoke(reward);
        HitToBreak--;
    }

    public void AddHits()
    {
        HitToBreak += 3;
    }
}
