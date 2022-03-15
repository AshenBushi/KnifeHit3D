using System.Collections;
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
        var seconds = 0.3f / Mathf.Abs(reward);

        while (_moneyCount != Player.Instance.Money)
        {
            if (reward > 0)
            {
                _moneyCount++;

                if (_moneyCount > Player.Instance.Money)
                {
                    _moneyCount = Player.Instance.Money;
                    _moneyText.text = Player.Instance.Money.ToString();
                    yield break;
                }
            }
            else
            {
                _moneyCount--;

                if (_moneyCount < Player.Instance.Money)
                {
                    _moneyCount = Player.Instance.Money;
                    _moneyText.text = Player.Instance.Money.ToString();
                    yield break;
                }
            }

            _moneyText.text = _moneyCount.ToString();
            yield return new WaitForSeconds(seconds);
        }
    }
}
