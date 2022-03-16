using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class DisablingAdsViewButton : AdButton
{
    [SerializeField] private DisablingAdsTimer _disablingAdsTimer;
    [SerializeField] private Slider _progressBar;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _progressBar.value = DataManager.Instance.GameData.DisablingAds.CounterAdsOff;

        Button.onClick.AddListener(WatchAd);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(WatchAd);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        DataManager.Instance.GameData.DisablingAds.CounterAdsOff++;
        DataManager.Instance.Save();

        _progressBar.value = DataManager.Instance.GameData.DisablingAds.CounterAdsOff;

        if (DataManager.Instance.GameData.DisablingAds.CounterAdsOff >= 2)
        {
            _disablingAdsTimer.EnableTimer();
        }

        base.HandleUserEarnReward(sender, e);
    }
}
