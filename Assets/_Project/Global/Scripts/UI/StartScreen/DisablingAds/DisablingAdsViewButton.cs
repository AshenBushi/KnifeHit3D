using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DisablingAdsViewButton : MonoBehaviour
{
    [SerializeField] private DisablingAdsTimer _disablingAdsTimer;
    [SerializeField] private Slider _progressBar;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _progressBar.value = DataManager.Instance.GameData.DisablingAds.CounterAdsOff;

        _button.onClick.AddListener(WatchAd);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(WatchAd);
    }

    public void WatchAd()
    {
        AdManager.Instance.ShowRewardVideo();

        DataManager.Instance.GameData.DisablingAds.CounterAdsOff++;
        DataManager.Instance.Save();

        _progressBar.value = DataManager.Instance.GameData.DisablingAds.CounterAdsOff;

        if (DataManager.Instance.GameData.DisablingAds.CounterAdsOff >= 2)
        {
            _disablingAdsTimer.EnableTimer();
        }
    }
}
