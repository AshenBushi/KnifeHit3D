using UnityEngine;
using UnityEngine.UI;

public class DisablingAdsScreen : UIScreen
{
    [SerializeField] private DisablingAdsTimer _disablingAdsTimer;
    [SerializeField] private Button _buttonContinue;

    public bool IsEnable { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _buttonContinue.onClick.AddListener(Disable);
        _disablingAdsTimer.IsTimeStart += Disable;
    }

    private void OnDisable()
    {
        _buttonContinue.onClick.RemoveListener(Disable);
        _disablingAdsTimer.IsTimeStart -= Disable;
    }

    public override void Enable()
    {
        IsEnable = true;
        base.Enable();
    }

    public override void Disable()
    {
        IsEnable = false;
        base.Disable();
    }
}
