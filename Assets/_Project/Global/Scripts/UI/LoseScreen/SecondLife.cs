using GoogleMobileAds.Api;
using UnityEngine;

public class SecondLife : AdButton
{
    [SerializeField] private ContinueScreen _continueScreen;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _continueScreen.Disable();
        KnifeHandler.Instance.SecondLife();

        base.HandleUserEarnReward(sender, e);
    }
}
