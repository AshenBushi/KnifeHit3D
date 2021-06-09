using GoogleMobileAds.Api;
using UnityEngine;

public class ContinuePlayLottery : AdButton
{
    [SerializeField] private LotteryHandler _lotteryHandler;

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        _lotteryHandler.ContinuePlay();

        base.HandleUserEarnReward(sender, e);
    }
    
    public override void WatchAd()
    {
        MetricaManager.SendEvent("btn_repeat");
        
        base.WatchAd();
    }
}
