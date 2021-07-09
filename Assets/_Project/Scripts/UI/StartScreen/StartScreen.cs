using UnityEngine;

public class StartScreen : UIScreen
{
    [SerializeField] private GameObject _giftNotification;
    [SerializeField] private GameObject _shopNotification;
    [SerializeField] private GameObject _panelNotification;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        CheckNotificationStates();
    }

    private void CheckNotificationStates()
    {
        if(DataManager.GameData.DailyGiftsData.UnlockedGifts > DataManager.GameData.DailyGiftsData.PickedGifts)
            EnableGiftNotification();
        
        if(PlayerPrefs.GetInt("ShopNotification") == 1)
            EnableShopNotification();
    }
    
    public void EnableGiftNotification()
    {
        _giftNotification.SetActive(true);
        _panelNotification.SetActive(true);
    }
    
    public void EnableShopNotification()
    {
        _shopNotification.SetActive(true);
        _panelNotification.SetActive(true);
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
        _panelNotification.SetActive(false);
    }
}
