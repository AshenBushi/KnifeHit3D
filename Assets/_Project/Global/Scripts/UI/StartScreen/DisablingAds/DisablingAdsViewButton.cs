using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class DisablingAdsViewButton : AdButton
{
    [SerializeField] private DisablingAdsTimer _disablingAdsTimer;
    [SerializeField] private Image _checkPoint1;
    [SerializeField] private Image _checkPoint2;

    private Color _color;
    private Color _enableColor;

    private void Awake()
    {
        Button = GetComponent<Button>();

        _color = _checkPoint1.color;
        _checkPoint2.color = _color;
        _enableColor = new Color(_color.r, _color.g, _color.b, 1f);
    }

    private void OnEnable()
    {
        if (DataManager.Instance.GameData.DisablingAds.CounterAdsOff == 1)
            _checkPoint1.color = _enableColor;

        Button.onClick.AddListener(WatchAd);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(WatchAd);
    }

    public override void WatchAd()
    {
        AdManager.Instance.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow += HandleFailedToShow;
        AdManager.Instance.ShowRewardVideo();
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        DataManager.Instance.GameData.DisablingAds.CounterAdsOff++;
        DataManager.Instance.Save();

        if (DataManager.Instance.GameData.DisablingAds.CounterAdsOff == 1)
            _checkPoint1.color = _enableColor;
        else if (DataManager.Instance.GameData.DisablingAds.CounterAdsOff >= 2)
        {
            _checkPoint2.color = _enableColor;

            _disablingAdsTimer.EnableTimer();

            SetDefaultColorCheckPoints();
        }

        base.HandleUserEarnReward(sender, e);
    }

    private void SetDefaultColorCheckPoints()
    {
        _checkPoint1.color = _color;
        _checkPoint2.color = _color;
    }
}
