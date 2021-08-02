using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemBuyer : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Shop _shop;
    [SerializeField] private MenuSwiper _menuSwiper;
    [SerializeField] private List<GameObject> _pages;
    [SerializeField] private float _animationDuration;
    [SerializeField] private int _iterationsCount;
    [SerializeField] private int _price;

    private Button _button;
    private List<ShopItem> _currentItems;
    private int _itemIndex;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _menuSwiper.IsPageChanged += OnPageChanged;
        _player.IsMoneyChanged += CheckPurchaseOpportunity;
        _button.onClick.AddListener(BuyItem);
    }

    private void OnDisable()
    {
        _menuSwiper.IsPageChanged -= OnPageChanged;
        _player.IsMoneyChanged -= CheckPurchaseOpportunity;
        _button.onClick.RemoveListener(BuyItem);
    }

    private void Start()
    {
        OnPageChanged();
    }

    private IEnumerator BuyAnimation()
    {
        var iterations = 0;

        _shop.CurrentItem.DisableIndicator();
        _button.interactable = false;
        _itemIndex = Random.Range(0, _currentItems.Count);

        if (_currentItems.Count > 1)
        {
            while (iterations != _iterationsCount)
            {
                SoundManager.Instance.PlaySound(SoundName.RandomBuy);
                _currentItems[_itemIndex].EnableIndicator();
                yield return new WaitForSeconds(_animationDuration / _iterationsCount);
                _currentItems[_itemIndex].DisableIndicator();

                var itemIndex = Random.Range(0, _currentItems.Count);

                while (itemIndex == _itemIndex)
                {
                    itemIndex = Random.Range(0, _currentItems.Count);
                }

                _itemIndex = itemIndex;
                iterations++;
            }
        }

        KnifeStorage.Instance.AddKnife(_currentItems[_itemIndex].Index);
        SoundManager.Instance.PlaySound(SoundName.RandomBuy);
        _currentItems[_itemIndex].Unlock();
        _currentItems[_itemIndex].SelectItem();
        _currentItems.Remove(_currentItems[_itemIndex]);
        CheckPurchaseOpportunity();
    }

    private void FindLockedItemsOnPage()
    {
        _currentItems = _pages[_menuSwiper.CurrentPage].GetComponentsInChildren<ShopItem>()
            .Where(item => !item.IsUnlock).ToList();
    }
    
    private void CheckPurchaseOpportunity()
    {
        if (_player.Money >= _price && _currentItems.Count > 0)
        {
            _button.interactable = true;
        }
        else
        {
            _button.interactable = false;
        }
    }

    private void OnPageChanged()
    {
        FindLockedItemsOnPage();
        CheckPurchaseOpportunity();
    }

    private void BuyItem()
    {
        MetricaManager.SendEvent("btn_buy");
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        FindLockedItemsOnPage();
        _player.WithdrawMoney(_price);
        StartCoroutine(BuyAnimation());
    }
}
