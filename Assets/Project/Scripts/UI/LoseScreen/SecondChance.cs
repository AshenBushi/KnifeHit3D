using GoogleMobileAds.Api;
using UnityEngine;

public class SecondChance : AdButton
{
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private KnifeSpawner _knifeSpawner;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _loseScreen.Disable();
        _knifeSpawner.SecondChance();
        
        base.HandleUserEarnReward(sender, e);
    }

    public override void WatchAd()
    {
        MetricaManager.SendEvent("btn_repeat");

        base.WatchAd();
    }
}
