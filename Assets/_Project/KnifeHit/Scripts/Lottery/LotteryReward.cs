using UnityEngine;

public class LotteryReward : MonoBehaviour
{
    [SerializeField] private RewardName _type;

    public RewardName Type => _type;
}
