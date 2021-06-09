using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

public class ShopScreen : UIScreen
{
    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }
}
