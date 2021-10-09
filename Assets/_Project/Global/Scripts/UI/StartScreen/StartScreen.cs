using UnityEngine;
using UnityEngine.Serialization;
using Watermelon;

public class StartScreen : UIScreen
{
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private HandlePages _handlerPages;
    [SerializeField] private GameObject _giftNotification;
    [SerializeField] private GameObject _shopNotification;
    //[SerializeField] private GameObject _settingsNotification;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _handlerPages.Init();
    }

    private void OnEnable()
    {
        CheckNotificationStates();
        
    }

    private void CheckNotificationStates()
    {
        if (DataManager.Instance.GameData.DailyGiftsData.UnlockedGifts > DataManager.Instance.GameData.DailyGiftsData.PickedGifts)
            EnableGiftNotification();

        if (PlayerPrefs.GetInt("ShopNotification") == 1)
            EnableShopNotification();
    }

    public override void Disable()
    {
        _settingsPanel.Hide(true);

        base.Disable();
    }

    public void EnableGiftNotification()
    {
        _giftNotification.SetActive(true);
        //_settingsNotification.SetActive(true);
    }

    public void EnableShopNotification()
    {
        _shopNotification.SetActive(true);
        //_settingsNotification.SetActive(true);
        PlayerPrefs.SetInt("ShopNotification", 1);
    }

    public void DisableGiftNotification()
    {
        _giftNotification.SetActive(false);
    }

    public void DisableShopNotification()
    {
        _shopNotification.SetActive(false);
        PlayerPrefs.SetInt("ShopNotification", 0);
    }

    public void DisablePanelNotification()
    {
        //_settingsNotification.SetActive(false);
    }
}
