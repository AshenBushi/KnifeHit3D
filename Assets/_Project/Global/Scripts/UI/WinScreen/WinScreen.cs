using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinScreen : UIScreen
{
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private Button _continue;
    [SerializeField] private ParticleSystem _particleCup;
    [SerializeField] private CanvasGroup _canvasGroup;

    private float _multiplierCutscene;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;

    public static WinScreen Instance;

    private void Awake()
    {
        Instance = this;
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Enable()
    {
        _particleCup.Play();
        //AdManager.Instance.Interstitial.OnAdClosed += HandleOnAdClosed;
        _continue.onClick.AddListener(OnClickContinue);

        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Win);

        StartCoroutine(DelayEnabledContinue());

        _rewardText.text = GamemodManager.Instance.CurrentMod == Gamemod.KnifeHit
            ? TargetType switch
            {
                0 => TargetHandler.Instance.CounterMoney.ToString(),
                1 => LevelManager.Instance.CurrentCubeLevel.Reward.ToString(),
                2 => LevelManager.Instance.CurrentFlatLevel.Reward.ToString(),
                _ => LevelManager.Instance.CurrentMarkLevel.Reward.ToString()
            }
            : "15";

        if (_multiplierCutscene != 0)
        {
            float tempReward = Convert.ToInt32(_rewardText.text);
            tempReward *= _multiplierCutscene;
            _rewardText.text = tempReward.ToString();
        }

        switch (GamemodManager.Instance.CurrentMod)
        {
            case Gamemod.StackKnife:
                LevelManager.Instance.NextStackKnifeLevel();
                break;
            case Gamemod.KnifeFest:
                LevelManager.Instance.NextKnifeFestLevel();
                break;
        }
    }

    public override void Disable()
    {
        _continue.onClick.RemoveListener(OnClickContinue);
        //AdManager.Instance.Interstitial.OnAdClosed -= HandleOnAdClosed;

        base.Disable();
        SessionHandler.Instance.EndSession();
    }

    public void OnWatchedReward(int coefficient)
    {
        int reward = Convert.ToInt32(_rewardText.text);
        _rewardText.text = (reward * coefficient).ToString();
    }

    public void Win()
    {
        Enable();
    }

    public void WinWithReward(float multiplierLastStep)
    {
        _multiplierCutscene = multiplierLastStep;
        Enable();
    }

    private void OnClickContinue()
    {
        StartCoroutine(OnClickContinueDelay());
    }

    private IEnumerator OnClickContinueDelay()
    {
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        _continue.interactable = false;

        var reward = Convert.ToInt32(_rewardText.text);
        Player.Instance.DepositMoney(reward);

        for (int i = reward; i >= 0; i--)
        {
            _rewardText.text = i.ToString();
            yield return null;
        }

        if (reward == 0)
            yield return null;
        else
            yield return new WaitForSeconds(0.7f);

        _continue.interactable = true;
        _continue.gameObject.SetActive(false);

        //var showIntAd = AdManager.Instance.ShowInterstitial();
        //DataManager.Instance.GameData.CanShowStartAd = showIntAd;
        //if (!showIntAd)
            Disable();
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
