using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    [SerializeField] private Sprite _enableSprite;
    [SerializeField] private Sprite _disableSprite;
    [SerializeField] private int _index;

    private ShopNavigation _navigation;
    private Button _button;
    private Image _image;

    public int Index => _index;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _navigation = GetComponentInParent<ShopNavigation>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        SoundManager.Instance.PlaySound(SoundName.ButtonClick);
        _navigation.SelectShopSection(this);
    }

    public void SetEnableSprite()
    {
        _image.sprite = _enableSprite;
    }
    
    public void SetDisableSprite()
    {
        _image.sprite = _disableSprite;
    }
}
