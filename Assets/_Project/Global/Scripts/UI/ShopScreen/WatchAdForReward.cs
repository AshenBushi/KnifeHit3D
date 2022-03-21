using GoogleMobileAds.Api;
using UnityEngine;

public class WatchAdForReward : AdButton
{
    [SerializeField] private int _moneyReward = 100;

    protected override void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        base.OnDisable();
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        Player.Instance.DepositMoney(_moneyReward);

        base.HandleUserEarnReward(sender, e);
    }

    public void OnClick()
    {
        WatchAd();

        if (IsAdFailed) return;

        _button.interactable = true;
    }
}
