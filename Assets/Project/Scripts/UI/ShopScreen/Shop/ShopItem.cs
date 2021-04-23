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
    [SerializeField] private GameObject _previewTemplate;

    private Button _button;
    private bool _isUnlock = false;
    
    public int Index => _index;

    public GameObject PreviewTemplate => _previewTemplate;

    public event UnityAction<ShopItem, GameObject> IsKnifeSelected;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _selectIndicator.GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(SelectKnife);
    }

    private void SelectKnife()
    {
        if (!_isUnlock) return;
        IsKnifeSelected?.Invoke(this, _previewTemplate);
    }

    public void Unlock()
    {
        _isUnlock = true;
        _lockIndicator.gameObject.SetActive(false);
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