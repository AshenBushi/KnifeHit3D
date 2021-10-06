using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private Image _selectIndicator;
    [SerializeField] private Image _lockIndicator;
    [SerializeField] private Image _unlockIndicator;
    [SerializeField] private GameObject _previewTemplate;
    [SerializeField] private Transform _container;

    private Button _button;
    private bool _isUnlock = false;
    
    public int Index => _index;

    public bool IsUnlock => _isUnlock;

    public event UnityAction<ShopItem> IsKnifeSelected;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _selectIndicator.GetComponentInChildren<Image>();
        Instantiate(KnifeStorage.Instance.KnifePreviews[Index], _container);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(SelectItem);
    }

    public void SelectItem()
    {
        if (!_isUnlock) return;
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        IsKnifeSelected?.Invoke(this);
    }

    public void Unlock()
    {
        _isUnlock = true;
        _lockIndicator.gameObject.SetActive(false);
        _unlockIndicator.gameObject.SetActive(true);
    }
    
    public void EnableIndicator()
    {
        _selectIndicator.gameObject.SetActive(true);
    }
    
    public void DisableIndicator()
    {
        _selectIndicator.gameObject.SetActive(false);
    }
}
