using GoogleMobileAds.Api;
using UnityEngine;

public class WinScreenAd : AdButton
{
    [SerializeField] private WinScreen _winScreen;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _winScreen.Disable();

        base.HandleUserEarnReward(sender, e);
    }
}
