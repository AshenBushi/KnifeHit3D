using GoogleMobileAds.Api;

public class ContinuePlayLottery : AdButton
{
    protected override void OnEnable()
    {
        _button.onClick.AddListener(WatchAd);
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        _button.onClick.RemoveListener(WatchAd);
        base.OnDisable();
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        LotteryHandler.Instance.ContinuePlay();

        base.HandleUserEarnReward(sender, e);
    }
}
