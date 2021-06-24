using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GiftScreen : UIScreen
{
    [SerializeField] private OpenGift _openGift;
    [SerializeField] private GameObject _giftModel;
    [SerializeField] private Button _continue;

    public event UnityAction IsScreenDisabled;
    public event UnityAction IsGiftOpened;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnGiftOpened()
    {
        IsGiftOpened?.Invoke();
        _openGift.IsGiftOpened -= OnGiftOpened;
        Disable();
    }
    
    public IEnumerator EnableAnimation()
    {
        Enable();

        yield return new WaitForSeconds(1f);

        _continue.gameObject.SetActive(true);
    }

    public void Continue()
    {
        Disable();
        IsScreenDisabled?.Invoke();
    }

    public void OpenGift()
    {
        _openGift.OpenGiftForAd();
        _openGift.IsGiftOpened += OnGiftOpened;
    }

    public override void Enable()
    {
        base.Enable();
        _giftModel.SetActive(true);
    }

    public override void Disable()
    {
        base.Disable();
        _giftModel.SetActive(false);
    }
}
