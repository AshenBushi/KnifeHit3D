using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillSkipTargetAd : AdButton
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ActivateSkip);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ActivateSkip);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _button.interactable = true;
        _button.image.color = Color.white;
    }

    public void Hide()
    {
        _button.interactable = false;
        _button.image.color = Color.grey;
    }

    private void Skip()
    {
        TargetHandler.Instance.CurrentTarget.BreakTarget();
        Hide();
    }

    private void ActivateSkip()
    {
        WatchAd();
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        Skip();

        base.HandleUserEarnReward(sender, e);
    }
}
