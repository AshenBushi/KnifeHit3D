using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _continue;
    [SerializeField] private TextMeshProUGUI _textReward;

    private Button _continueButton;

    public static LoseScreen Instance;

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
        SoundManager.Instance.PlaySound(SoundName.Lose);

        StartCoroutine(DelayEnabledContinue());

        if (GamemodManager.Instance.CurrentMod == Gamemod.KnifeHit)
            _textReward.text = TargetHandler.Instance.CounterMoney.ToString();
        else _textReward.text = "10";
    }

    public void OnWatchedReward(int coefficient)
    {
        Player.Instance.WithdrawMoney(Convert.ToInt32(_textReward.text));
        int reward = Convert.ToInt32(_textReward.text);
        _textReward.text = (reward * coefficient).ToString();
        Player.Instance.DepositMoney(Convert.ToInt32(_textReward.text));
    }

    public override void Disable()
    {
        _continueButton.onClick.RemoveListener(OnClickContinue);
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
        _continueButton.interactable = false;

        var reward = Convert.ToInt32(_textReward.text);
        Player.Instance.DepositMoney(reward);

        float timeSec = 0.08f;
        for (int i = reward; i >= 0; i--)
        {
            _textReward.text = i.ToString();
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
