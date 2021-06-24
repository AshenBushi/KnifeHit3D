using UnityEngine;

public class StartScreen : UIScreen
{
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private GameObject _giftNotification;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if(DataManager.GameData.DailyGiftsData.UnlockedGifts > DataManager.GameData.DailyGiftsData.PickedGifts)
            _giftNotification.SetActive(true);
    }

    public void EnableSettingsScreen()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        _settingsScreen.Enable();
    }
    
    public void EnableShopScreen()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        _shopScreen.Enable();
    }
}
