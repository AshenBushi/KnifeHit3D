using DG.Tweening;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DoubleReward : AdButton
{
    [SerializeField] private GameObject _circleCoefficients;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private List<RotateDefinition> _definitions;

    private int _coefficient = 1;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;

    public event UnityAction<int> IsWatchedReward;

    private void OnEnable()
    {
        RotateCircle();
    }

    protected override void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        MetricaManager.SendEvent("ev_rew_fail");
        base.HandleFailedToShow(sender, e);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        if (GamemodManager.Instance.CurrentMod == Gamemod.KnifeHit)
            switch (TargetType)
            {
                case 0:
                    Player.Instance.DepositMoney(LevelManager.Instance.CurrentMarkLevel.Reward * _coefficient);
                    break;
                case 1:
                    Player.Instance.DepositMoney(LevelManager.Instance.CurrentCubeLevel.Reward * _coefficient);
                    break;
                case 2:
                    Player.Instance.DepositMoney(LevelManager.Instance.CurrentFlatLevel.Reward * _coefficient);
                    break;
                default:
                    Player.Instance.DepositMoney(LevelManager.Instance.CurrentMarkLevel.Reward * _coefficient);
                    break;
            }

        MetricaManager.SendEvent("ev_rew_show");

        base.HandleUserEarnReward(sender, e);
    }

    private void RotateCircle()
    {
        var currentDefinition = _definitions[Random.Range(0, _definitions.Count)];
        var rotateEuler = new Vector3(0f, 0f, currentDefinition.Angle);

        _circleCoefficients.transform.DORotate(_circleCoefficients.transform.eulerAngles + rotateEuler, currentDefinition.Duration, RotateMode.FastBeyond360)
            .SetEase(currentDefinition.EaseCurve).OnComplete(RotateCircle);
    }

    public void CheckArrowCoefficient()
    {
        var pos = new Vector3(_circleCoefficients.transform.position.x, _circleCoefficients.transform.position.y + 70f, _circleCoefficients.transform.position.z);
        Ray ray = new Ray(pos, _circleCoefficients.transform.forward * 100);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 150))
        {
            var obj = hit.collider.gameObject.GetComponent<WinLotterySection>();

            if (obj != null)
                _coefficient = obj.Type switch
                {
                    WinLotterySectionType.x2 => 2,
                    WinLotterySectionType.x3 => 3,
                    _ => 1,
                };
        }

        Debug.Log(_coefficient);
    }
}
