using GoogleMobileAds.Api;
using UnityEngine;

public class ContinuePlayLottery : AdButton
{
    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        LotteryHandler.Instance.ContinuePlay();

        base.HandleUserEarnReward(sender, e);
    }
    
    public override void WatchAd()
    {
        MetricaManager.SendEvent("btn_repeat");
        
        base.WatchAd();
    }
}
