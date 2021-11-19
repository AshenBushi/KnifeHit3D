using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandlePages : Singleton<HandlePages>, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _duration;
    [SerializeField] private RectTransform _content;
    [SerializeField] private List<RectTransform> _menuItems = new List<RectTransform>();
    [SerializeField] private RectTransform _center;

    private List<Page> _menuPages = new List<Page>();
    private HorizontalLayoutGroup _layoutGroup;
    private Vector3 _stockScalePage;
    private Vector3 _minimalScalePage = new Vector3(0.7f, 0.7f, 1f);
    private int _fastScrollIndex = 0;
    private bool _isFastScroll = false;

    public int CurrentPage { get; private set; } = 0;
    public List<Page> MenuPages => _menuPages;

    protected override void Awake()
    {
        base.Awake();

        var indexGameMod = 1;
        for (var i = 0; i < _menuItems.Count; i++)
        {
            _menuPages.Add(_menuItems[i].GetComponent<Page>());

            if (i <= GamemodManager.Instance.KnifeHitModsCount)
            {
                _menuPages[i].SetKnifeMod(i);
                _menuPages[i].SetGameMod(0);
            }
            else
            {
                _menuPages[i].SetGameMod(indexGameMod);
                if (indexGameMod <= GamemodManager.Instance.GameModCount)
                    indexGameMod++;
            }
        }

        _stockScalePage = _menuPages[0].transform.localScale;
    }

    private void Start()
    {
        _layoutGroup = _content.GetComponent<HorizontalLayoutGroup>();

        SelectCurrentPage(0f);
    }

    private void SelectCurrentPage(float duration)
    {
        var endValue = new Vector3(-_menuItems[CurrentPage].localPosition.x +
                                   (_menuItems[CurrentPage].sizeDelta.x +
                                    _layoutGroup.spacing * (_layoutGroup.padding.left / (_layoutGroup.spacing / 2))) / 2, 0f, 0f);

        _content.DOLocalMove(endValue, duration);

        for (var i = 0; i < _menuItems.Count; i++)
        {
            if (i != CurrentPage)
                _menuItems[i].transform.DOScale(_minimalScalePage, duration);
            else
                _menuItems[i].transform.DOScale(_stockScalePage, duration);
        }
    }

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
            if (CurrentPage + _fastScrollIndex >= 0 && CurrentPage + _fastScrollIndex < _menuItems.Count)
            {
                CurrentPage += _fastScrollIndex;
            }
        }
        else
        {
            var currentDistance = float.MaxValue;

            for (var i = 0; i < _menuItems.Count; i++)
            {
                if (Vector3.Distance(_menuItems[i].position, _center.position) < currentDistance)
                {
                    currentDistance = Vector3.Distance(_menuItems[i].position, _center.position);

                    CurrentPage = i;
                }
            }

        }

        SelectCurrentPage(_duration);
    }

    public void SelectNextPage()
    {
        if (CurrentPage >= _menuItems.Count - 1)
            CurrentPage = 0;
        else
            CurrentPage++;

        SelectCurrentPage(_duration);
    }

    public void SelectPreviousPage()
    {
        if (CurrentPage <= 0)
            CurrentPage = _menuItems.Count - 1;
        else
            CurrentPage--;

        SelectCurrentPage(_duration);
    }

    public void DisallowPageKnifeFest()
    {
        for (int i = 0; i < _menuPages.Count; i++)
        {
            if (_menuPages[i].GameMod == 2)
            {
                _menuPages[i].DisallowButton();
                break;
            }
        }
    }
}