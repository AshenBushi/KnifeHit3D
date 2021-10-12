using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LotteryScreen : UIScreen
{
    [SerializeField] private LotteryRewarder _lotteryRewarder;
    [SerializeField] private List<TMP_Text> _rewardsTexts;

    private List<RewardName> Rewards => LotteryHandler.Instance.Rewards;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Enable()
    {
        base.Enable();
        ShowRewards();
    }

    public void Collect()
    {
        _lotteryRewarder.SendRewards(Rewards);
    }

    private void ShowRewards()
    {
        for (var i = 0; i < Rewards.Count; i++)
        {
            _rewardsTexts[i].text = Rewards[i] switch
            {
                RewardName.TenCoins => "10 coins",
                RewardName.TwentyCoins => "20 coins",
                RewardName.ThirtyCoins => "30 coins",
                RewardName.Slow => "Slow",
                RewardName.LevelPass => "Level Pass",
                RewardName.SecondChance => "Second Chance",
                RewardName.Skin => "Skin",
                RewardName.Death => "",
                _ => throw new ArgumentOutOfRangeException()
            };

            _rewardsTexts[i].GetComponentInParent<Image>().DOFade(1, 0.1f);
        }
    }
}
