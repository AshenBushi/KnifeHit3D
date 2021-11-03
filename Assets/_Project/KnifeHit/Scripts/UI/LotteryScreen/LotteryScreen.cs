using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LotteryScreen : UIScreen
{
    [SerializeField] private LotteryRewarder _lotteryRewarder;
    [SerializeField] private List<TMP_Text> _rewardsTexts;
    [SerializeField] private Button _buttonCollect;

    private List<RewardName> Rewards => LotteryHandler.Instance.Rewards;
    private List<TMP_Text> _textCoins;

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
        StartCoroutine(OnClickCollectDelay());
    }

    private void ShowRewards()
    {
        _textCoins = new List<TMP_Text>();

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

            GettingCoinsText(i);

            _rewardsTexts[i].GetComponentInParent<Image>().DOFade(1, 0.1f);
        }
    }

    private void GettingCoinsText(int i)
    {
        if (Rewards[i] == RewardName.TenCoins || Rewards[i] == RewardName.TwentyCoins || Rewards[i] == RewardName.ThirtyCoins)
            _textCoins.Add(_rewardsTexts[i]);
    }

    private IEnumerator OnClickCollectDelay()
    {
        _buttonCollect.interactable = false;
        _lotteryRewarder.SendRewards(Rewards);

        float timeSec = 0.08f;
        for (int j = 0; j < _textCoins.Count; j++)
        {
            for (int i = Convert.ToInt32(_textCoins[j].text); i >= 0; i--)
            {
                _textCoins[j].text = i.ToString();
                yield return new WaitForSeconds(timeSec);

                if (timeSec > 0.01f)
                    timeSec -= 0.005f;
            }
        }

        yield return new WaitForSeconds(1f);

        _buttonCollect.interactable = true;
        SessionHandler.Instance.EndSession();
    }
}
