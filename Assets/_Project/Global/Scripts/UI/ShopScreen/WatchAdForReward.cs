using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WatchAdForReward : MonoBehaviour
{
    [SerializeField] private int _moneyReward = 100;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(WatchAd);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(WatchAd);
    }

    private void WatchAd()
    {
        AdManager.Instance.ShowRewardVideo();

        Player.Instance.DepositMoney(_moneyReward);
    }
}
