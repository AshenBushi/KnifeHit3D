using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class LoseScreen : UIScreen, IRewardedVideoAdListener
{
    [SerializeField] private Button _continue;
    [SerializeField] private TextMeshProUGUI _textReward;
    [SerializeField] private ParticleSystem _particleCup;

    public static LoseScreen Instance;

    private void Awake()
    {
        Instance = this;
        CanvasGroup = GetComponent<CanvasGroup>();
        AdManager.Instance.OnRewardedCallback += SetCallback;
    }

    public override void Enable()
    {
        _particleCup.Play();
        //AdManager.Instance.Interstitial.OnAdClosed += HandleOnAdClosed;
        _continue.onClick.AddListener(OnClickContinue);

        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Lose);

        StartCoroutine(DelayEnabledContinue());

        if (GamemodManager.Instance.CurrentMod == Gamemod.KnifeHit)
            _textReward.text = TargetHandler.Instance.CounterMoney.ToString();
        else
            _textReward.text = "10";
    }

    public void OnWatchedReward(int coefficient)
    {
        int reward = Convert.ToInt32(_textReward.text);
        _textReward.text = (reward * coefficient).ToString();
    }

    public override void Disable()
    {
        AdManager.Instance.OnRewardedCallback -= SetCallback;
        _continue.onClick.RemoveListener(OnClickContinue);
        //AdManager.Instance.Interstitial.OnAdClosed -= HandleOnAdClosed;

        base.Disable();
        SessionHandler.Instance.EndSession();
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
            yield return null;
        else
            yield return new WaitForSeconds(0.7f);

        _continue.interactable = true;
        _continue.gameObject.SetActive(false);

        var showIntAd = AdManager.Instance.ShowInterstitial();
        DataManager.Instance.GameData.CanShowStartAd = showIntAd;
        if (!showIntAd)
            Disable();
    }

    private IEnumerator DelayEnabledContinue()
    {
        yield return new WaitForSeconds(4f);
        _continue.gameObject.SetActive(true);
    }

    //private void HandleOnAdClosed(object sender, EventArgs e)
    //{
    //    MetricaManager.SendEvent("int_show");
    //    Disable();
    //}

    private void SetCallback()
    {
        Appodeal.setRewardedVideoCallbacks(this);
    }

    public void onRewardedVideoLoaded(bool precache)
    {
        throw new NotImplementedException();
    }

    public void onRewardedVideoFailedToLoad()
    {
        throw new NotImplementedException();
    }

    public void onRewardedVideoShowFailed()
    {
        throw new NotImplementedException();
    }

    public void onRewardedVideoShown()
    {
        throw new NotImplementedException();
    }

    public void onRewardedVideoFinished(double amount, string name)
    {
        MetricaManager.SendEvent("int_show");
        Disable();
    }

    public void onRewardedVideoClosed(bool finished)
    {
        throw new NotImplementedException();
    }

    public void onRewardedVideoExpired()
    {
        throw new NotImplementedException();
    }

    public void onRewardedVideoClicked()
    {
        throw new NotImplementedException();
    }
}
