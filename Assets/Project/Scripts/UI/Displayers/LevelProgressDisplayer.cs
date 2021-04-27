using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplayer : MonoBehaviour
{
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private List<Image> _points;
    [SerializeField] private Color _enabledColor;

    private int _currentPoint = 0;

    private void OnEnable()
    {
        _startScreen.IsModChanged += ShowLevelNumber;
    }

    private void OnDisable()
    {
        _startScreen.IsModChanged -= ShowLevelNumber;
    }

    private void Start()
    {
        ShowLevelNumber();
        NextPoint();
    }

    private void ShowLevelNumber()
    {
        _levelText.text = DataManager.GameData.ProgressData.CurrentGamemod switch
        {
            0 => (DataManager.GameData.ProgressData.CurrentTargetLevel + 1).ToString(),
            1 => (DataManager.GameData.ProgressData.CurrentCubeLevel + 1).ToString(),
            2 => (DataManager.GameData.ProgressData.CurrentFlatLevel + 1).ToString(),
            _ => (DataManager.GameData.ProgressData.CurrentTargetLevel + 1).ToString()
        };
    }

    public void NextPoint()
    {
        _points[_currentPoint].color = _enabledColor;
        _currentPoint++;
    }
}
