using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class HandlePages : MonoBehaviour
{
    [SerializeField] private RectTransform _contentRect;
    [SerializeField] private Page _templatePage;
    [SerializeField] private int _countMods;
    [SerializeField] private float _offsetX = 1100f;

    private Page[] _mods;
    private List<Vector2> _modsPos = new List<Vector2>();
    private Vector2 _contentVector;
    private int _currentIndexPage;
    private int _indexSelectedMod;
    private bool _isDrag;

    public Page[] Mods => _mods;

    public int CurrentIndexPage
    {
        get => _currentIndexPage;

        private set
        {
            if (_mods[_currentIndexPage] != null)
            {
                Page aktivesObj = _mods[_currentIndexPage];
                aktivesObj.Deactivation();
            }

            if (value < 0)
                _currentIndexPage = _mods.Length - 1;
            else if (value > _mods.Length - 1)
                _currentIndexPage = 0;
            else
                _currentIndexPage = value;

            if (_mods[_currentIndexPage] != null)
            {
                Page aktivesObj = _mods[_currentIndexPage];
                aktivesObj.Activation();
                _contentRect.DOAnchorPosX(_modsPos[_currentIndexPage].x, 1f);
            }
        }
    }

    private void FixedUpdate()
    {
        float maxPos = float.MaxValue;
        for (int i = 0; i < _mods.Length; i++)
        {
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - _modsPos[i].x);
            if (distance < maxPos)
            {
                maxPos = distance;
                _indexSelectedMod = i;
            }
        }

        if (_isDrag) return;

        _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _modsPos[_indexSelectedMod].x, 10 * Time.fixedDeltaTime);
        _contentRect.anchoredPosition = _contentVector;
        _currentIndexPage = _indexSelectedMod;
        if (_currentIndexPage > 0)
            _mods[_currentIndexPage - 1].Deactivation();
        if (_currentIndexPage < _mods.Length - 1)
            _mods[_currentIndexPage + 1].Deactivation();

        _mods[_currentIndexPage].Activation();
    }

    public void Init()
    {
        _mods = new Page[_countMods];

        int indexGameMod = 1;
        for (int i = 0; i < _countMods; i++)
        {
            _mods[i] = Instantiate(_templatePage, _contentRect.transform, false);
            _mods[i].InitMovie(i + 1);

            if (i <= GamemodManager.Instance.KnifeHitModsCount)
            {
                _mods[i].SetKnifeMod(i);
                _mods[i].SetGameMod(0);
            }
            else
            {
                _mods[i].SetGameMod(indexGameMod);
                if (indexGameMod <= GamemodManager.Instance.GameModCount)
                    indexGameMod++;
            }

            if (i != 0)
            {
                _mods[i].Deactivation();
                _mods[i].transform.localPosition = new Vector2(_mods[i - 1].transform.localPosition.x + _templatePage.GetComponent<RectTransform>().sizeDelta.x + _offsetX, 10f);
            }
            else
                _mods[i].transform.localPosition = new Vector2(_templatePage.transform.localPosition.x, 10f);

            _modsPos.Add(-_mods[i].transform.localPosition);
        }

        _mods[0].Activation();
    }

    public void SnapScrolling(bool isDrag)
    {
        _isDrag = isDrag;
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