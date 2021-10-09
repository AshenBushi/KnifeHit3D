using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class HandlePages : MonoBehaviour
{
    [SerializeField] private RectTransform _contentRect;
    [SerializeField] private Page _templatePage;
    [SerializeField] private int _countMods;
    [SerializeField] private float _offsetX = 1500f;

    private Page[] _mods;
    private List<Vector2> _modsPos = new List<Vector2>();
    private int _currentIndexPage;
    private bool _isScrolled = false;

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
            }
        }
    }
    private void FixedUpdate()
    {
        _contentRect.DOAnchorPosX(_modsPos[CurrentIndexPage].x, 0.4f);
    }

    public void Init()
    {
        _mods = new Page[_countMods];

        int indexGameMod = 1;
        for (int i = 0; i < _countMods; i++)
        {
            _mods[i] = Instantiate(_templatePage, _contentRect.transform, false);

            if (i <= GamemodManager.Instance.KnifeHitModsCount)
            {
                _mods[i].SetKnifeMod(i);
            }
            else
            {
                _mods[i].SetGameMod(indexGameMod);
                if (indexGameMod < GamemodManager.Instance.GameModCount)
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