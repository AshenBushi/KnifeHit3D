using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Transform _container;
    [SerializeField] private Image _pointTemplate;
    [SerializeField] private Image _bossTemplate;
    [SerializeField] private Color _enabledColor;

    private readonly List<Image> _points = new List<Image>();
    private int _currentPoint = 0;

    public void ShowLevelDisplay()
    {
        _levelText.text = TargetHandler.Instance.CurrentSpawnerIndex switch
        {
            0 => (DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + 1).ToString(),
            1 => (DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + 1).ToString(),
            2 => (DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + 1).ToString(),
            3 => (DataManager.Instance.GameData.ProgressData.CurrentMark2Level + 1).ToString(),
            4 => (DataManager.Instance.GameData.ProgressData.CurrentCube2Level + 1).ToString(),
            5 => (DataManager.Instance.GameData.ProgressData.CurrentFlat2Level + 1).ToString(),
            _ => " "
        };

        if (_points.Count > 0)
        {
            foreach (var point in _points)
            {
                Destroy(point.gameObject);
            }

            _points.Clear();
        }

        if (TargetHandler.Instance.CurrentSpawnerIndex == 1)
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
