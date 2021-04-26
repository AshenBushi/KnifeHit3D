using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private HitScoreDisplayer _hitScoreDisplayer;
    [SerializeField] private LevelProgressDisplayer _levelProgressDisplayer;
    [SerializeField] private Target _targetTemplate;
    [SerializeField] private float _spawnZ;
    [SerializeField] private float _spawnStep;
    [SerializeField] private float _spawnY;
    [SerializeField] private float _animationDuration;
    
    private List<Target> _targets = new List<Target>();
    private Target _currentTarget;
    private Tween _tween;

    public event UnityAction IsWin;

    private void OnDisable()
    {
        if (_currentTarget == null) return;
        _currentTarget.IsBreak -= OnTargetBreak;
        _currentTarget.IsTakeHit -= OnTargetTakeHit;
    }

    private void OnTargetBreak(TargetBase targetBase)
    {
        _currentTarget.IsBreak -= OnTargetBreak;
        _currentTarget.IsTakeHit -= OnTargetTakeHit;
        _hitScoreDisplayer.SubmitHit();
        StartCoroutine(TargetBreakAnimation(targetBase));
    }

    private void OnTargetTakeHit()
    {
        _player.AllowThrow();
        _hitScoreDisplayer.SubmitHit();
    }

    private void Move()
    {
        foreach (var target in _targets)
        {
            var position = target.transform.position;
            _tween = target.transform.DOMove(new Vector3(position.x, position.y, position.z - _spawnStep), _animationDuration);
        }

        _tween.OnComplete(() =>
        {
            _player.AllowThrow();
        });
    }

    private IEnumerator TargetBreakAnimation(TargetBase targetBase)
    {
        targetBase.Detonate();
        
        _targets.Remove(_currentTarget);
        
        if (_targets.Count > 0)
        {
            _levelProgressDisplayer.NextPoint();
            SetCurrentTarget();
            Move();
        }
        else
        {
            IsWin?.Invoke();
        }
        
        yield return new WaitForSeconds(2f);
        
        Destroy(targetBase.gameObject);
    }
    
    public void SetCurrentTarget()
    {
        _currentTarget = _targets[0];
        _currentTarget.IsBreak += OnTargetBreak;
        _currentTarget.IsTakeHit += OnTargetTakeHit;
        _hitScoreDisplayer.SpawnHitScores(_currentTarget.HitToBreak);
    }
    
    public void SpawnLevel(TargetLevel targetLevel, Knife obstacleTemplate)
    {
        for (var i = 0; i < targetLevel.Targets.Count; i++)
        {
            _targets.Add(Instantiate(_targetTemplate, new Vector3(0f, _spawnY, _spawnZ + _spawnStep * i), Quaternion.identity, transform));
            _targets[i].SpawnAndSetup(targetLevel.Targets[i], obstacleTemplate);
        }
    }

    public void Reload(TargetLevel targetLevel, Knife obstacleTemplate)
    {
        for (var i = 0; i < targetLevel.Targets.Count; i++)
        {
            _targets[i].ReinitializeObstacle(targetLevel.Targets[i], obstacleTemplate);
        }
    }
}
