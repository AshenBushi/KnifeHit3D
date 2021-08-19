using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    
    private int _moneyCount;

    private void OnEnable()
    {
        Player.Instance.IsMoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        Player.Instance.IsMoneyChanged -= OnMoneyChanged;
    }

    private void Start()
    {
        _moneyCount = Player.Instance.Money;
        _moneyText.text = _moneyCount.ToString();
    }

    private void OnMoneyChanged()
    {
        _moneyText.text = _moneyCount.ToString();
        StartCoroutine(AccrueReward());
    }
    
    private IEnumerator AccrueReward()
    {
        var reward = Player.Instance.Money - _moneyCount;

        while (_moneyCount != Player.Instance.Money)
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
