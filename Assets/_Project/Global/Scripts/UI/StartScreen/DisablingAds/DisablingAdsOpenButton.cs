using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DisablingAdsOpenButton : MonoBehaviour
{
    [SerializeField] private DisablingAdsScreen _disablingAdsScreen;
    [SerializeField] private DisablingAdsTimer _disablingAdsTimer;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (DataManager.Instance.GameData.DisablingAds.IsAdsDisableOneDay)
        {
            DisableButton();
            _disablingAdsTimer.IsTimeEnd += EnableButton;
            return;
        }

        _button.onClick.AddListener(ActivationScreen);
        _disablingAdsTimer.IsTimeStart += DisableButton;

    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ActivationScreen);

        _disablingAdsTimer.IsTimeStart -= DisableButton;
        _disablingAdsTimer.IsTimeEnd -= EnableButton;
    }

    public void DisableButton()
    {
        _button.interactable = false;
    }

    public void EnableButton()
    {
        _button.interactable = true;
    }

    private void ActivationScreen()
    {
        if (_disablingAdsScreen.IsEnable)
            _disablingAdsScreen.Disable();
        else
            _disablingAdsScreen.Enable();
    }
}
