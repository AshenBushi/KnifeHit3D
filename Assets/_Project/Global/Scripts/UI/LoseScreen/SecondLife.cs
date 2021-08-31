using GoogleMobileAds.Api;
using UnityEngine;

public class SecondLife : AdButton
{
    [SerializeField] private LoseScreen _loseScreen;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _loseScreen.Disable();
        KnifeHandler.Instance.SecondLife();
        
        base.HandleUserEarnReward(sender, e);
    }

    public override void WatchAd()
    {
        MetricaManager.SendEvent("btn_repeat");

        base.WatchAd();
    }
}
