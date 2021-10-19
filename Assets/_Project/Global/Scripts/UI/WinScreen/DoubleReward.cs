using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Collections.Generic;

public class DoubleReward : AdButton
{
    [SerializeField] private GameObject _circleCoefficients;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private List<RotateDefinition> _definitions;

    //private float[] _durations = { 65f, 55f, 75f };
    //private int _chanceGiveX5;
    private int _coefficient = 1;

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;

    public event UnityAction<int> IsWatchedReward;

    private void OnEnable()
    {
        RotateCircle();
        //_chanceGiveX5 = Random.Range(0, 100);

        //_coefficient = _chanceGiveX5 < 20 ? 4 : 1;
        //_text.text = "Watch X" + (1 + _coefficient).ToString();
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
        //var duration = Random.Range(0, _durations.Length);

        var currentDefinition = _definitions[Random.Range(0, _definitions.Count)];
        var rotateEuler = new Vector3(0f, 0f, currentDefinition.Angle);

        _circleCoefficients.transform.DORotate(_circleCoefficients.transform.eulerAngles + rotateEuler, currentDefinition.Duration, RotateMode.FastBeyond360)
            .SetEase(currentDefinition.EaseCurve).OnComplete(RotateCircle);

        //_circleCoefficients.transform.DORotate(_circleCoefficients.transform.eulerAngles + new Vector3(0, 0, 360f), duration, RotateMode.FastBeyond360).SetDelay(0.5f).SetEase(Ease.OutCubic).OnComplete(RotateCircle);
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
                switch (obj.Type)
                {
                    case WinLotterySectionType.x2:
                        _coefficient = 2;
                        break;
                    case WinLotterySectionType.x3:
                        _coefficient = 3;
                        break;
                    default:
                        _coefficient = 1;
                        break;
                }
        }

        Debug.Log(_coefficient);
    }
}
