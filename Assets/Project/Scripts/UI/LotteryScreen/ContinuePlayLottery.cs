using GoogleMobileAds.Api;
using UnityEngine;

public class ContinuePlayLottery : AdButton
{
    [SerializeField] private LotteryScreen _lotteryScreen;
    [SerializeField] private KnifeSpawner _knifeSpawner;
    [SerializeField] private LotterySpawner _lotterySpawner;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _lotteryScreen.Disable();
        _lotterySpawner.ReplayLottery();
        _knifeSpawner.ReplayLottery();
        
        base.HandleUserEarnReward(sender, e);
    }
    
    public override void WatchAd()
    {
        MetricaManager.SendEvent("btn_repeat");
        
        base.WatchAd();
    }
}
