using System.Collections.Generic;
using UnityEngine;

public class ShopNavigation : Singleton<ShopNavigation>
{
    [SerializeField] private Shop _shop;
    [SerializeField] private List<NavigationButton> _buttons;

    public void SelectShopSection(NavigationButton button)
    {
        _shop.EnableShopSection(button.Index);

        for (var i = 0; i < _buttons.Count; i++)
        {
            if (_buttons[i] == button) continue;
            _buttons[i].transform.SetSiblingIndex(i);
            _buttons[i].SetDisableSprite();
        }

        button.transform.SetAsLastSibling();
        button.SetEnableSprite();
    }

    public void SelectShopSectionOnFirstOpened()
    {
        SelectShopSection(_buttons[0]);
    }
}
