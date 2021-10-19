using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LotterySpawner : MonoBehaviour
{
    [SerializeField] private Lottery _template;
    [SerializeField] private Image _background;
    [SerializeField] private Button _buttonBack;

    private readonly Vector3 _lotteryPosition = new Vector3(0f, -2.25f, 8f);

    public Lottery SpawnLottery()
    {
        _background.gameObject.SetActive(true);
        _buttonBack.gameObject.SetActive(true);

        var lottery = Instantiate(_template, _lotteryPosition, Quaternion.Euler(0f, 180f, 0f), transform);

        return lottery;
    }

    public void DeactivationBackground()
    {
        _background.gameObject.SetActive(false);
        _buttonBack.gameObject.SetActive(false);
    }
}
