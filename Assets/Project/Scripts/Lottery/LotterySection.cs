using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LotterySection : MonoBehaviour
{
    [SerializeField] private RewardName _name;

    public event UnityAction<RewardName> IsRewardTook;

    public void TakeReward()
    {
        IsRewardTook?.Invoke(_name);
    }
}
