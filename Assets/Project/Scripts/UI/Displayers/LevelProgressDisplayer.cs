using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private List<Image> _points;
    [SerializeField] private Color _enabledColor;

    private int _currentPoint = 0;

    private void Start()
    {
        _levelText.text = (DataManager.GameData.ProgressData.CurrentLevel + 1).ToString();
        NextPoint();
    }

    public void NextPoint()
    {
        _points[_currentPoint].color = _enabledColor;
        _currentPoint++;
    }
}
