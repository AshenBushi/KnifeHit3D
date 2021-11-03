using GoogleMobileAds.Api;
using UnityEngine;

public class LoseScreenAd : AdButton
{
    [SerializeField] private LoseScreen _loseScreen;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _loseScreen.Disable();

        base.HandleUserEarnReward(sender, e);
    }
}
