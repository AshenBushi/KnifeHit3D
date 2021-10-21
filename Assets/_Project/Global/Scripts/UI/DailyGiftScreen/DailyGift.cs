using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyGift : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _moneyReward;
    [SerializeField] private int _knifeRewardIndex;
    [SerializeField] private Sprite _picked;
    [SerializeField] private TMP_Text _buttonText;

    private DailyGiftArrows _dailyArrows;

    public DailyGiftArrows DailyArrows { get => _dailyArrows; set => _dailyArrows = value; }

    public event UnityAction<bool, int> IsGotReward;

    public void Unlock()
    {
        _button.interactable = true;
    }

    public void Pick()
    {
        _button.transition = Selectable.Transition.None;
        _button.interactable = false;
        _buttonText.text = "";
        _button.GetComponent<Image>().sprite = _picked;
    }

    public void Get(bool isMoney)
    {
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);

        if (isMoney)
        {
            IsGotReward?.Invoke(false, _moneyReward);
        }
        else
        {
            IsGotReward?.Invoke(true, _knifeRewardIndex);
        }

        Pick();
    }

    public void SetPositionArrows(bool isEffectMoving)
    {
        if (isEffectMoving)
            _dailyArrows.AllowMove();
        else
            _dailyArrows.DisallowMove();

        _dailyArrows.SetParent(_button.gameObject.transform.parent);
        _dailyArrows.SetPosition(-(_button.gameObject.transform.parent.GetComponent<RectTransform>().rect.height / 2));
    }
}