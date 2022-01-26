using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisablingAdsScreen : UIScreen
{
    [SerializeField] private DisablingAdsTimer _disablingAdsTimer;
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private TMP_Text _textProgress;

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

    private void Update()
    {
        if (DataManager.Instance.GameData.DisablingAds.CounterAdsOff > 0 && DataManager.Instance.GameData.DisablingAds.CounterAdsOff <= 2)
            _textProgress.text = DataManager.Instance.GameData.DisablingAds.CounterAdsOff + "/2";
    }

    public override void Enable()
    {
        base.Enable();
        IsEnable = true;
        _textProgress.text = DataManager.Instance.GameData.DisablingAds.CounterAdsOff + "/2";
    }

    public override void Disable()
    {
        base.Disable();
        IsEnable = false;
    }
}
