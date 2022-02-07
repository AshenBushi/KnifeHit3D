using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Button))]
public class DoubleReward : MonoBehaviour
{
    [SerializeField] private GameObject _circleCoefficients;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private List<RotateDefinition> _definitions;

    private Button _button;
    private int _coefficient = 1;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        RotateCircle();
    }

    public void WatchAd()
    {
        AdManager.Instance.ShowRewardVideo();
        _button.interactable = false;
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
