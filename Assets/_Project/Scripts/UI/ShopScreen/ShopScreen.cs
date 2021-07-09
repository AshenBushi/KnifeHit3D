using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

public class ShopScreen : UIScreen
{
    [SerializeField] private StartScreen _startScreen;
    
    public override void Enable()
    {
        gameObject.SetActive(true);
        _startScreen.DisableShopNotification();
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }
}
