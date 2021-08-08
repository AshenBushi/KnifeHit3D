using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DoubleReward : AdButton
{
    [SerializeField] private TMP_Text _text;

    private int _chanceGiveX5;
    private int _coefficient;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;
    
    public event UnityAction IsWatchedReward;

    private void OnEnable()
    {
        _chanceGiveX5 = Random.Range(0, 100);

        _coefficient = _chanceGiveX5 < 20 ? 4 : 1;
        _text.text = "Watch X" + (1 + _coefficient).ToString();
    }

    protected override void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        MetricaManager.SendEvent("ev_rew_fail");
        base.HandleFailedToShow(sender, e);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        switch (TargetType)
        {
            case 0 :
                Player.Instance.DepositMoney(LevelManager.Instance.CurrentMarkLevel.Reward * _coefficient);
                break;
            case 1 :
                Player.Instance.DepositMoney(LevelManager.Instance.CurrentCubeLevel.Reward * _coefficient);
                break;
            case 2 :
                Player.Instance.DepositMoney(LevelManager.Instance.CurrentFlatLevel.Reward * _coefficient);
                break;
            default:
                Player.Instance.DepositMoney(LevelManager.Instance.CurrentMarkLevel.Reward * _coefficient);
                break;
        }
        
        MetricaManager.SendEvent("ev_rew_show");
        IsWatchedReward?.Invoke();
        
        base.HandleUserEarnReward(sender, e);
    }
}
