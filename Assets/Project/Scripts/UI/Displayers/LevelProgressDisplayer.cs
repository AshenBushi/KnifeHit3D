using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplayer : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private List<Image> _points;
    [SerializeField] private Color _enabledColor;

    private int _currentPoint = 0;

    private void Start()
    {
        _level.text = (_levelManager.CurrentIndex + 1).ToString();
        NextPoint();
    }

    public void NextPoint()
    {
        _points[_currentPoint].color = _enabledColor;
        _currentPoint++;
    }
}
