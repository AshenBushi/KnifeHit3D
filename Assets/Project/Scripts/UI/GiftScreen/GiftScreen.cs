using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GiftScreen : UIScreen
{
    [SerializeField] private GameObject _giftModel;
    [SerializeField] private Button _continue;

    public event UnityAction IsScreenDisabled;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator EnableAnimation()
    {
        Enable();

        yield return new WaitForSeconds(1f);

        _continue.gameObject.SetActive(true);
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
        IsScreenDisabled?.Invoke();
    }
}
