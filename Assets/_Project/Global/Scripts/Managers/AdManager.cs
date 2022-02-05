using AppodealAds.Unity.Api;
using UnityEngine;
using UnityEngine.Events;

public class AdManager : Singleton<AdManager>
{
    [SerializeField] private bool _isTestMode;

    public static string _appKey = "7a5387621aed368ae2d74a388446d0cc43de6ee5bf059882";
    public UnityAction OnRewardedCallback;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Appodeal.setTesting(_isTestMode);
        Appodeal.initialize(_appKey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO | Appodeal.BANNER_BOTTOM);

        OnRewardedCallback?.Invoke();
    }

    public bool ShowInterstitial()
    {
        if (Appodeal.canShow(Appodeal.INTERSTITIAL))
        {
            Appodeal.show(Appodeal.INTERSTITIAL);
            MetricaManager.SendEvent("int_start");
            return true;
        }

        return false;
    }

    public bool ShowRewardVideo()
    {
        if (Appodeal.canShow(Appodeal.REWARDED_VIDEO))
        {
            Appodeal.show(Appodeal.REWARDED_VIDEO);
            MetricaManager.SendEvent("rew_start");
            return true;
        }

        return false;
    }

    public void ShowBanner()
    {
        Appodeal.show(Appodeal.BANNER_BOTTOM);
    }

    public void HideBanner()
    {
        Appodeal.hide(Appodeal.BANNER_BOTTOM);
    }
}
