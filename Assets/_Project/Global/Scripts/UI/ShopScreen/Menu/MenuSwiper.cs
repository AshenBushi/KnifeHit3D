using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _duration;
    [SerializeField] private RectTransform _content;

    private HorizontalLayoutGroup _layoutGroup;
    private MItem[] _menuItems;
    private PageIndicator[] _navigationPoints;
    private List<RectTransform> _menuItemTransforms = new List<RectTransform>();
    private int _currentPage = 0;
    private int _fastScrollIndex = 0;
    private bool _isFastScroll = false;

    public int CurrentPage => _currentPage;

    public event UnityAction IsPageChanged;

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
            if ((_currentPage + _fastScrollIndex) >= 0 && (_currentPage + _fastScrollIndex) < _menuItems.Length)
            {
                _menuItems[_currentPage].gameObject.SetActive(false);
                _currentPage += _fastScrollIndex;
                _menuItems[_currentPage].gameObject.SetActive(true);
            }
        }
        else
        {
            var currentX = float.MaxValue;

            for (var i = 0; i < _menuItemTransforms.Count; i++)
            {
                if (!(Mathf.Abs(_menuItemTransforms[i].position.x) < currentX))
                {
                    _menuItems[i].gameObject.SetActive(false);
                    continue;
                }
                currentX = Mathf.Abs(_menuItemTransforms[i].position.x);
                _currentPage = i;
                _menuItems[i].gameObject.SetActive(true);
            }
        }

        IsPageChanged?.Invoke();

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
            if (_currentPage == i)
            {
                _navigationPoints[i].EnablePoint();
            }
            else
            {
                _navigationPoints[i].DisablePoint();
            }
        }

        _content.DOLocalMove(new Vector3(-_menuItemTransforms[_currentPage].localPosition.x +
                                         (_menuItemTransforms[_currentPage].sizeDelta.x +
                                          _layoutGroup.spacing) / 2, 0f, 0f), duration);
    }
}