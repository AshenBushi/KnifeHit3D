using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private ProgressPoint _pointTemplate;

    private List<ProgressPoint> _points;

    private int _hitCount;

    private void Clear()
    {
        if(_points != null)
            foreach (var point in _points)
            {
                Destroy(point.gameObject);
            }

        _points = new List<ProgressPoint>();
        _hitCount = 0;
    }
    
    public void SpawnProgressBar(int pointCount)
    {
        Clear();

        for (var i = 0; i < pointCount; i++)
        {
            _points.Add(Instantiate(_pointTemplate, transform));
        }
    }

    public void SubmitHit()
    {
        _points[_hitCount].TurnOn();
        _hitCount++;
    }
}
