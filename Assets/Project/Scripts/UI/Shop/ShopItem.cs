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

    private Button _button;
    
    public int Index => _index;
    public event UnityAction<ShopItem> IsKnifeSelected;

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
        IsKnifeSelected?.Invoke(this);
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
