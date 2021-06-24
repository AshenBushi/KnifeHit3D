using GoogleMobileAds.Api;
using Project.Scripts.Handlers;
using UnityEngine;

public class SecondLife : AdButton
{
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private KnifeHandler _knifeHandler;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _loseScreen.Disable();
        _knifeHandler.SecondLife();
        
        base.HandleUserEarnReward(sender, e);
    }

    public override void WatchAd()
    {
        MetricaManager.SendEvent("btn_repeat");

        base.WatchAd();
    }
}
