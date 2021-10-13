using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<ShopSection> _shopSections;
    [SerializeField] private ItemPreview _itemPreview;

    private List<ShopItem> _shopItems;
    private ShopItem _currentItem;
    private int _startIndex = 1;

    public ShopItem CurrentItem => _currentItem;

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

        if (PlayerPrefs.HasKey("ShopNavigationNotification"))
            ShopNavigation.Instance.EnableNavigationNotification(PlayerPrefs.GetInt("ShopNavigationNotification"));
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
        foreach (var item in _shopItems.Where(item => DataManager.Instance.GameData.ShopData.OpenedKnives.Contains(item.Index)))
        {
            item.Unlock();

            if (item.Index != DataManager.Instance.GameData.ShopData.CurrentKnifeIndex) continue;
            OnItemSelected(item);
        }
    }

    private void OnItemSelected(ShopItem item)
    {
        if (_currentItem != null)
            _currentItem.DisableIndicator();

        _currentItem = item;

        _itemPreview.SpawnSelectedItem(KnifeStorage.Instance.KnifePreviews[_currentItem.Index]);

        _currentItem.EnableIndicator();

        KnifeStorage.Instance.ChangeKnife(_currentItem.Index);
    }

    public void EnableShopSection(int index)
    {
        for (var i = 0; i < _shopSections.Count; i++)
            if (i != index)
                _shopSections[i].gameObject.SetActive(false);

        _shopSections[index].gameObject.SetActive(true);
    }
}
