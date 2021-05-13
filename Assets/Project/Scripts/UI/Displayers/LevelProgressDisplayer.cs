using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplayer : MonoBehaviour
{
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Transform _container;
    [SerializeField] private Image _pointTemplate;
    [SerializeField] private Image _bossTemplate;
    [SerializeField] private Color _enabledColor;

    private List<Image> _points = new List<Image>();
    private int _currentPoint = 0;

    private void OnEnable()
    {
        _startScreen.IsModChanged += ShowLevelDisplay;
    }

    private void OnDisable()
    {
        _startScreen.IsModChanged -= ShowLevelDisplay;
    }

    private void Start()
    {
        ShowLevelDisplay();
    }

    private void ShowLevelDisplay()
    {
        _levelText.text = DataManager.GameData.ProgressData.CurrentGamemod switch
        {
            0 => (DataManager.GameData.ProgressData.CurrentTargetLevel + 1).ToString(),
            1 => (DataManager.GameData.ProgressData.CurrentCubeLevel + 1).ToString(),
            2 => (DataManager.GameData.ProgressData.CurrentFlatLevel + 1).ToString(),
            _ => (DataManager.GameData.ProgressData.CurrentTargetLevel + 1).ToString()
        };

        if (_points.Count > 0)
        {
            foreach (var point in _points)
            {
                Destroy(point.gameObject);
            }
            
            _points.Clear();
        }
        
        if (DataManager.GameData.ProgressData.CurrentGamemod == 1)
        {
            for (var i = 0; i < 5; i++)
            {
                _points.Add(Instantiate(_pointTemplate, _container));
            }
        }
        else
        {
            for (var i = 0; i < 4; i++)
            {
                _points.Add(Instantiate(_pointTemplate, _container));
            }
        }
        
        _points.Add(Instantiate(_bossTemplate, _container));
        _currentPoint = 0;
        NextPoint();
    }

    public void NextPoint()
    {
        _points[_currentPoint].color = _enabledColor;
        _currentPoint++;
    }

    public void DisableDisplayer()
    {
        gameObject.SetActive(false);
    }
}
