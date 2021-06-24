using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyDisplayer : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _moneyText;

    private Tween _tween;
    private int _moneyCount;

    private void OnEnable()
    {
        _player.IsMoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _player.IsMoneyChanged -= OnMoneyChanged;
    }

    private void Start()
    {
        _moneyCount = _player.Money;
        _moneyText.text = _moneyCount.ToString();
    }

    private void OnMoneyChanged()
    {
        _moneyText.text = _moneyCount.ToString();
        StartCoroutine(AccrueReward());
    }
    
    private IEnumerator AccrueReward()
    {
        var reward = _player.Money - _moneyCount;

        while (_moneyCount != _player.Money)
        {
            if (reward > 0)
            {
                _moneyCount++;
            }
            else
            {
                _moneyCount--;
            }
            
            _moneyText.text = _moneyCount.ToString();
            yield return new WaitForSeconds(.3f / Mathf.Abs(reward));
        }
    }
}
