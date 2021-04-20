﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _duration;
    [SerializeField] private RectTransform _content;
    [SerializeField] private ItemShop _itemShop;

    private HorizontalLayoutGroup _layoutGroup;
    private MItem[] _menuItems;
    private PageIndicator[] _navigationPoints;
    private List<RectTransform> _menuItemTransforms = new List<RectTransform>();
    private Tween _tween;
    private int _fastScrollIndex = 0;
    private bool _isFastScroll = false;

    public void OnDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.delta.x) > 2)
        {
            if (eventData.delta.x < 0)
                _fastScrollIndex = 1;
            else
                _fastScrollIndex = -1;
            _isFastScroll = true;
        }
        else
        {
            _isFastScroll = false;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isFastScroll)
        {
            if ((_itemShop.CurrentPage + _fastScrollIndex) >= 0 && (_itemShop.CurrentPage + _fastScrollIndex) < _menuItems.Length)
            {
                _itemShop.CurrentPage += _fastScrollIndex;
            }
        }
        else
        {
            var currentX = float.MaxValue;

            for (var i = 0; i < _menuItemTransforms.Count; i++)
            {
                if (!(Mathf.Abs(_menuItemTransforms[i].position.x) < currentX)) continue;
                currentX = Mathf.Abs(_menuItemTransforms[i].position.x);
                _itemShop.CurrentPage = i;
            }
        }

        SelectCurrentPage(_duration);
    }

    private void OnEnable()
    {
        _layoutGroup = _content.GetComponent<HorizontalLayoutGroup>();
        _menuItems = _content.GetComponentsInChildren<MItem>();
        _navigationPoints = GetComponentsInChildren<PageIndicator>();
        
        foreach (var item in _menuItems)
        {
            _menuItemTransforms.Add(item.GetComponent<RectTransform>());
        }
        
        SelectCurrentPage(0f);
    }

    private void SelectCurrentPage(float duration)
    {
        for (var i = 0; i < _navigationPoints.Length; i++)
        {
            if (_itemShop.CurrentPage == i)
            {
                _navigationPoints[i].EnablePoint();
            }
            else
            {
                _navigationPoints[i].DisablePoint();
            }
        }
        
        _tween = _content
            .DOLocalMove(new Vector3(-_menuItemTransforms[_itemShop.CurrentPage].localPosition.x +
                                     (_menuItemTransforms[_itemShop.CurrentPage].sizeDelta.x + 
                                      _layoutGroup.spacing) / 2, _content.position.y, _content.position.z), duration);
    }
}