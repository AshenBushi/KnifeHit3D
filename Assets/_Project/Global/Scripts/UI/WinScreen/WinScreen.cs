using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WinScreen : UIScreen
{
    [SerializeField] private GameObject _cup;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private DoubleReward _doubleReward;

    private float _multiplierCutscene;
    private bool _isShowedDoubleReward = false;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _doubleReward.IsWatchedReward += OnWatchedReward;
    }

    private void OnDisable()
    {
        _doubleReward.IsWatchedReward -= OnWatchedReward;
    }

    private void OnWatchedReward()
    {
        _isShowedDoubleReward = true;
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Win);
        _cup.SetActive(true);

        _rewardText.text = GamemodManager.Instance.CurrentMod == 0
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
    }

    public override void Disable()
    {
        base.Disable();
        _cup.SetActive(false);
        IsScreenDisabled?.Invoke(true);
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
}
