using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoseScreen : UIScreen
{
    [SerializeField] private Button _continue;
    [SerializeField] private TextMeshProUGUI _textReward;

    public static LoseScreen Instance;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        Instance = this;

        CanvasGroup = GetComponent<CanvasGroup>();

        AdManager.Instance.Interstitial.OnAdClosed += HandleOnAdClosed;
        _continue.onClick.AddListener(OnClickContinue);
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Lose);

        StartCoroutine(DelayEnabledContinue());

        if (GamemodManager.Instance.CurrentMod == Gamemod.KnifeHit)
            _textReward.text = TargetHandler.Instance.CounterMoney.ToString();
        else _textReward.text = "10";
    }

    public void OnWatchedReward(int coefficient)
    {
        int reward = Convert.ToInt32(_textReward.text);
        _textReward.text = (reward * coefficient).ToString();
    }

    public override void Disable()
    {
        _continue.onClick.RemoveListener(OnClickContinue);
        AdManager.Instance.Interstitial.OnAdClosed -= HandleOnAdClosed;
        base.Disable();
        IsScreenDisabled?.Invoke(false);
    }

    public void Lose()
    {
        Enable();
    }

    private void OnClickContinue()
    {
        StartCoroutine(OnClickContinueDelay());
    }

    private IEnumerator OnClickContinueDelay()
    {
        _continue.interactable = false;

        var reward = Convert.ToInt32(_textReward.text);
        Player.Instance.DepositMoney(reward);

        for (int i = reward; i >= 0; i--)
        {
            _textReward.text = i.ToString();
            yield return null;
        }

        if (reward == 0)
            yield return new WaitForSeconds(0.1f);
        else
            yield return new WaitForSeconds(1.5f);

        AdManager.Instance.ShowInterstitial();
    }

    private IEnumerator DelayEnabledContinue()
    {
        yield return new WaitForSeconds(4f);
        _continue.gameObject.SetActive(true);
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        MetricaManager.SendEvent("int_show");
        Disable();
    }
}
