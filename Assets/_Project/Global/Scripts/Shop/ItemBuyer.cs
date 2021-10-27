using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemBuyer : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private MenuSwiper _menuSwiper;
    [SerializeField] private List<GameObject> _pages;
    [SerializeField] private float _animationDuration;
    [SerializeField] private int _iterationsCount;
    [SerializeField] private int _price;

    private Button _button;
    private List<ShopItem> _currentLockedItems;
    private int _itemIndex;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _menuSwiper.IsPageChanged += OnPageChanged;
        Player.Instance.IsMoneyChanged += CheckPurchaseOpportunity;
        _button.onClick.AddListener(BuyItem);
    }

    private void OnDisable()
    {
        _menuSwiper.IsPageChanged -= OnPageChanged;
        Player.Instance.IsMoneyChanged -= CheckPurchaseOpportunity;
        _button.onClick.RemoveListener(BuyItem);
    }

    private void Start()
    {
        OnPageChanged();
    }

    private IEnumerator BuyAnimation()
    {
        var iterations = 0;

        if (_shop.CurrentItem != null)
            _shop.CurrentItem.DisableIndicator();

        _button.interactable = false;
        _itemIndex = Random.Range(0, _currentLockedItems.Count);

        if (_currentLockedItems.Count > 1)
        {
            while (iterations != _iterationsCount)
            {
                SoundManager.Instance.PlaySound(SoundName.RandomBuy);
                _currentLockedItems[_itemIndex].EnableIndicator();
                yield return new WaitForSeconds(_animationDuration / _iterationsCount);
                _currentLockedItems[_itemIndex].DisableIndicator();

                var itemIndex = Random.Range(0, _currentLockedItems.Count);

                while (itemIndex == _itemIndex)
                {
                    itemIndex = Random.Range(0, _currentLockedItems.Count);
                }

                _itemIndex = itemIndex;
                iterations++;
            }
        }

        KnifeStorage.Instance.AddKnife(_currentLockedItems[_itemIndex].Index);
        SoundManager.Instance.PlaySound(SoundName.RandomBuy);
        _currentLockedItems[_itemIndex].Unlock();
        _currentLockedItems[_itemIndex].SelectItem();
        _currentLockedItems.Remove(_currentLockedItems[_itemIndex]);
        CheckPurchaseOpportunity();
    }

    private void FindLockedItemsOnPage()
    {
        _currentLockedItems = _pages[_menuSwiper.CurrentPage].GetComponentsInChildren<ShopItem>()
            .Where(item => !item.IsUnlock).ToList();
    }

    private void CheckPurchaseOpportunity()
    {
        if (Player.Instance.Money >= _price && _currentLockedItems.Count > 0)
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
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        FindLockedItemsOnPage();
        Player.Instance.WithdrawMoney(_price);
        StartCoroutine(BuyAnimation());
    }
}
