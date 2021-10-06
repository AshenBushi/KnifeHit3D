using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class HandlePages : MonoBehaviour
{
    [SerializeField] private RectTransform _contentRect;
    [SerializeField] private Page _templatePage;
    [SerializeField] private int _countMods;
    [SerializeField] private float _offsetX = 1500f;

    private List<Page> _mods = new List<Page>();
    private List<Vector2> _modsPos = new List<Vector2>();
    private int _currentIndexPage;

    public int CurrentIndexPage
    {
        get => _currentIndexPage;
        set
        {
            if (_mods[_currentIndexPage] != null)
            {
                Page aktivesObj = _mods[_currentIndexPage];
                aktivesObj.Deactivation();
            }

            if (value < 0)
                _currentIndexPage = _mods.Count - 1;
            else if (value > _mods.Count - 1)
                _currentIndexPage = 0;
            else
                _currentIndexPage = value;
            if (_mods[_currentIndexPage] != null)
            {
                Page aktivesObj = _mods[_currentIndexPage];
                aktivesObj.Activation();
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < _countMods; i++)
        {
            _mods.Add(Instantiate(_templatePage, _contentRect.transform, false));
            _mods[i].SetTextNum(i);

            if (i != 0)
            {
                _mods[i].Deactivation();
                _mods[i].transform.localPosition = new Vector2(_mods[i - 1].transform.localPosition.x + _templatePage.GetComponent<RectTransform>().sizeDelta.x + _offsetX, 0);
            }
            else
                _mods[i].transform.localPosition = new Vector2(_templatePage.transform.localPosition.x, 0);

            _modsPos.Add(-_mods[i].transform.localPosition);
        }
    }

    private void FixedUpdate()
    {
        _contentRect.DOAnchorPosX(_modsPos[CurrentIndexPage].x, 0.15f);
    }

    public void SelectNextMod()
    {
        CurrentIndexPage++;
    }

    public void SelectPrevMod()
    {
        CurrentIndexPage--;
    }
}
