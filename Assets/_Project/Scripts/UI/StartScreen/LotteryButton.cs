using UnityEngine;
using UnityEngine.UI;

public class LotteryButton : MonoBehaviour
{
    [SerializeField] private LotteryTimer _lotteryTimer;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.interactable = DataManager.Instance.GameData.IsLotteryEnable;
    }

    private void OnEnable()
    {
        _lotteryTimer.IsTimeEnd += EnableButton;
        _lotteryTimer.IsTimeStart += DisableButton;
    }

    private void OnDisable()
    {
        _lotteryTimer.IsTimeEnd -= EnableButton;
        _lotteryTimer.IsTimeStart -= DisableButton;
    }

    private void EnableButton()
    {
        _button.interactable = true;
    }
    
    private void DisableButton()
    {
        _button.interactable = false;
    }
}
