using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

public class ShopScreen : UIScreen
{
    [SerializeField] private GameObject _uiCanvas;
    [SerializeField] private StartScreen _startScreen;
    
    public override void Enable()
    {
        gameObject.SetActive(true);
        _startScreen.DisableShopNotification();
        _uiCanvas.SetActive(false);
    }

    public override void Disable()
    {
        _uiCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
