using DG.Tweening;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoubleReward : AdButton
{
    [SerializeField] private GameObject _circleCoefficients;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private List<RotateDefinition> _definitions;

    private int _coefficient = 1;

    private void OnEnable()
    {
        RotateCircle();
    }

    protected override void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        base.HandleFailedToShow(sender, e);
    }

    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        base.HandleUserEarnReward(sender, e);
        Button.interactable = false;
    }

    public void CheckArrowCoefficient()
    {
        var pos = new Vector3(_circleCoefficients.transform.position.x, _arrow.transform.position.y, _circleCoefficients.transform.position.z);
        Ray ray = new Ray(pos, _arrow.transform.forward * 150);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200))
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
    }

    public void DoubleWinReward()
    {
        WinScreen.Instance.OnWatchedReward(_coefficient);
    }

    public void DoubleLoseReward()
    {
        LoseScreen.Instance.OnWatchedReward(_coefficient);
    }

    private void RotateCircle()
    {
        var currentDefinition = _definitions[Random.Range(0, _definitions.Count)];
        var rotateEuler = new Vector3(0f, 0f, currentDefinition.Angle);

        _circleCoefficients.transform.DORotate(_circleCoefficients.transform.eulerAngles + rotateEuler, currentDefinition.Duration, RotateMode.FastBeyond360)
            .SetEase(currentDefinition.EaseCurve).OnComplete(RotateCircle).SetLink(gameObject);
    }
}
