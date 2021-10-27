using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdForReward : AdButton
{
    private int _moneyReward;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        Player.Instance.DepositMoney(_moneyReward);

        base.HandleUserEarnReward(sender, e);
    }

    public void WatchAd(int value)
    {
        _moneyReward = value;

        WatchAd();

        Button.interactable = true;
    }
}
