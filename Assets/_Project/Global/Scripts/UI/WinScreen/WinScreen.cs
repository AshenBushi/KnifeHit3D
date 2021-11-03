using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinScreen : UIScreen
{
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private GameObject _continue;

    private Button _continueButton;
    private float _multiplierCutscene;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;

    public static WinScreen Instance;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        Instance = this;

        CanvasGroup = GetComponent<CanvasGroup>();

        _continueButton = _continue.GetComponent<Button>();
        _continueButton.onClick.AddListener(OnClickContinue);
    }

    public override void Enable()
    {
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
        _continueButton.onClick.RemoveListener(OnClickContinue);
        base.Disable();
        _continue.SetActive(false);
        IsScreenDisabled?.Invoke(false);
    }

    public void OnWatchedReward(int coefficient)
    {
        Player.Instance.WithdrawMoney(Convert.ToInt32(_rewardText.text));
        int reward = Convert.ToInt32(_rewardText.text);
        _rewardText.text = (reward * coefficient).ToString();
        Player.Instance.DepositMoney(Convert.ToInt32(_rewardText.text));
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
        _continueButton.interactable = false;

        var reward = Convert.ToInt32(_rewardText.text);
        Player.Instance.DepositMoney(reward);

        float timeSec = 0.08f;
        for (int i = reward; i >= 0; i--)
        {
            _rewardText.text = i.ToString();
            yield return new WaitForSeconds(timeSec);

            if (timeSec > 0.01f)
                timeSec -= 0.005f;
        }

        if (reward == 0)
            yield return new WaitForSeconds(0.1f);
        else
            yield return new WaitForSeconds(1.5f);

        _continueButton.GetComponent<AdButton>().WatchAd();
    }

    private IEnumerator DelayEnabledContinue()
    {
        yield return new WaitForSeconds(4f);
        _continue.SetActive(true);
    }
}
