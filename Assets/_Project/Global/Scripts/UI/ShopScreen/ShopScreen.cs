using UnityEngine;

public class ShopScreen : UIScreen
{
    [SerializeField] private GameObject _canvasPlayerInput;
    [SerializeField] private GameObject _uiCanvas;
    [SerializeField] private StartScreen _startScreen;

    public override void Enable()
    {
        gameObject.SetActive(true);
        _startScreen.DisableShopNotification();
        _uiCanvas.SetActive(false);
        _canvasPlayerInput.SetActive(false);
        ShopNavigation.Instance.SelectShopSectionOnFirstOpened();
    }

    public override void Disable()
    {
        _uiCanvas.SetActive(true);
        _canvasPlayerInput.SetActive(true);
        gameObject.SetActive(false);
    }
}
