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
    private Image _buttonImage;

    public DailyGiftArrows DailyArrows { get => _dailyArrows; set => _dailyArrows = value; }

    public event UnityAction<bool, int> IsGotReward;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
    }

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

        var _buttonParentRect = _button.gameObject.transform.parent.GetComponent<RectTransform>();

        _dailyArrows.SetParent(_buttonParentRect);
        _dailyArrows.transform.SetAsFirstSibling();
        _dailyArrows.SetPosition(-(_buttonParentRect.rect.height / 2));
    }
}