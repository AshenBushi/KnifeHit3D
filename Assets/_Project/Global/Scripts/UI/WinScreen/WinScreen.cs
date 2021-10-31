using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WinScreen : UIScreen
{
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private GameObject _buttonContinue;

    private float _multiplierCutscene;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;

    public static WinScreen Instance;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        Instance = this;

        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Win);

        StartCoroutine(DelayEnabledContinue());

        _rewardText.text = GamemodManager.Instance.CurrentMod == Gamemod.KnifeHit
            ? TargetType switch
            {
                0 => LevelManager.Instance.CurrentMarkLevel.Reward.ToString(),
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

        Player.Instance.DepositMoney(Convert.ToInt32(_rewardText.text));

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
        base.Disable();
        _buttonContinue.SetActive(false);
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

    public void Continue()
    {
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);

        Disable();
    }

    private IEnumerator DelayEnabledContinue()
    {
        yield return new WaitForSeconds(4f);
        _buttonContinue.SetActive(true);
    }
}
