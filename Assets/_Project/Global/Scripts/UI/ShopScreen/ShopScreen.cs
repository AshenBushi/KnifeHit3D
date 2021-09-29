using System;
using GoogleMobileAds.Api;
using KnifeFest;
using UnityEngine;
using UnityEngine.Events;

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
        _canvasPlayerInput.GetComponentInChildren<PlayerInput>().DisallowTap();
        _canvasPlayerInput.SetActive(false);
        if (GamemodManager.Instance.CurrentMod == Gamemod.KnifeFest)
            CursorTracker.Instance.DisableCanvas();
    }

    public override void Disable()
    {
        _uiCanvas.SetActive(true);
        _canvasPlayerInput.SetActive(true);
        gameObject.SetActive(false);
        _canvasPlayerInput.GetComponentInChildren<PlayerInput>().AllowTap();
        if (GamemodManager.Instance.CurrentMod == Gamemod.KnifeFest)
            CursorTracker.Instance.EnableCanvas();
    }
}
