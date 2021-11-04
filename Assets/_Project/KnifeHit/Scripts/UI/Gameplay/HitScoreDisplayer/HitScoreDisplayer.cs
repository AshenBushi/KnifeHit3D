using System.Collections.Generic;
using UnityEngine;

public class HitScoreDisplayer : MonoBehaviour
{
    [SerializeField] private HitScore _scoreTemplate;

    private List<HitScore> _scores;

    private int _hitCount;

    private void Clear()
    {
        if(_scores != null)
            foreach (var point in _scores)
            {
                Destroy(point.gameObject);
            }

        _scores = new List<HitScore>();
        _hitCount = 0;
    }
    
    public void SpawnHitScores(int pointCount)
    {
        Clear();

        for (var i = 0; i < pointCount; i++)
        {
            _scores.Add(Instantiate(_scoreTemplate, transform));
        }
    }

    public void SubmitHit()
    {
        _scores[_hitCount].TurnOn();
        _hitCount++;
    }
}
