using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _continue;
    [SerializeField] private TextMeshProUGUI _textReward;

    public static LoseScreen Instance;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        Instance = this;

        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Lose);

        _textReward.text = "10";

        Player.Instance.DepositMoney(Convert.ToInt32(_textReward.text));
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
        base.Disable();
        IsScreenDisabled?.Invoke(false);
    }

    public void Lose()
    {
        Enable();
    }
}
