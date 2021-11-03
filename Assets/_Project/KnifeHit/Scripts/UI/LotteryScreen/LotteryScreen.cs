using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        for (int i = 0; i < Rewards.Count; i++)
        {
            _rewardsTexts[i].text = Rewards[i] switch
            {
                RewardName.TenCoins => "10",
                RewardName.TwentyCoins => "20",
                RewardName.ThirtyCoins => "30",
                RewardName.Slow => "",
                RewardName.LevelPass => "",
                RewardName.SecondChance => "",
                RewardName.Skin => "",
                RewardName.Death => "",
                _ => ""
            };

            for (int j = 0; j < _rewardsTexts[i].transform.childCount; j++)
            {
                var rewards = _rewardsTexts[i].transform.GetChild(j).GetComponents<LotteryReward>();
                for (int l = 0; l < rewards.Length; l++)
                {
                    if (Rewards[i] == rewards[l].Type)
                    {
                        rewards[l].gameObject.SetActive(true);
                        break;
                    }
                }
            }

            _rewardsTexts[i].GetComponentInParent<Image>().DOFade(1, 0.1f);
        }
    }
}
