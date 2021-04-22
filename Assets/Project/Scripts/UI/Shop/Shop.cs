using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<ShopSection> _shopSections;
    
    private List<ShopItem> _shopItems;
    private ShopItem _currentItem;
    private int _currentItemIndex;
    private int _startIndex = 1;

    private void Awake()
    {
        _shopItems = GetComponentsInChildren<ShopItem>().ToList();
        EnableShopSection(_startIndex);
    }

    private void OnEnable()
    {
        foreach (var item in _shopItems)
        {
            item.IsKnifeSelected += OnItemSelected;
        }
    }

    private void OnDisable()
    {
        foreach (var item in _shopItems)
        {
            item.IsKnifeSelected -= OnItemSelected;
        }
    }

    private void Start()
    {
        foreach (var item in _shopItems.Where(item => item.Index == DataManager.GameData._shopData.CurrentKnifeIndex))
        {
            _currentItem = item;
            _currentItemIndex = DataManager.GameData._shopData.CurrentKnifeIndex;
            _currentItem.EnableIndicator();
        }
    }

    private void OnItemSelected(ShopItem item)
    {
        _currentItem.DisableIndicator();
        _currentItem = item;
        _currentItemIndex = item.Index;
        DataManager.GameData._shopData.CurrentKnifeIndex = _currentItemIndex;
        DataManager.Save();
        _currentItem.EnableIndicator();
    }

    public void EnableShopSection(int index)
    {
        for (var i = 0; i < _shopSections.Count; i++)
            if (i != index)
                _shopSections[i].gameObject.SetActive(false);
        
        _shopSections[index].gameObject.SetActive(true);
    }
}
