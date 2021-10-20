using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class LoseScreen : UIScreen
{
    [SerializeField] private GameObject _continue;
    [SerializeField] private TextMeshProUGUI _textReward;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;

    public event UnityAction<bool> IsScreenDisabled;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Enable()
    {
        base.Enable();
        SoundManager.Instance.PlaySound(SoundName.Lose);

        _textReward.text =
            //GamemodManager.Instance.CurrentMod == Gamemod.KnifeHit
            //? TargetType switch
            //{
            //    0 => (LevelManager.Instance.CurrentMarkLevel.Reward / 3).ToString(),
            //    1 => (LevelManager.Instance.CurrentCubeLevel.Reward / 3).ToString(),
            //    2 => (LevelManager.Instance.CurrentFlatLevel.Reward / 3).ToString(),
            //    _ => (LevelManager.Instance.CurrentMarkLevel.Reward / 3).ToString()
            //}
            //:
                "5";

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
