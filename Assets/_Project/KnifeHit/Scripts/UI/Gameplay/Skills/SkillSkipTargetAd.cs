//using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillSkipTargetAd : AdButton
{
    //[SerializeField] private Image _adIcon;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        //_button.onClick.AddListener(ActivateSkip);

        //if (DataManager.Instance.GameData.PlayerData.LevelPass <= 0)
        //    _adIcon.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        //_button.onClick.RemoveListener(ActivateSkip);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Skip()
    {
        TargetHandler.Instance.CurrentTarget.BreakTarget();
        Hide();
    }

    //private void ActivateSkip()
    //{
    //    WatchAd();
    //}

    //protected override void HandleUserEarnReward(object sender, Reward e)
    //{
    //    Skip();

    //    base.HandleUserEarnReward(sender, e);
    //}
}
