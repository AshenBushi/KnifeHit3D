using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<TargetBase> _targetBases = new List<TargetBase>();
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private Target _targetTemplate;
    [SerializeField] private float _spawnStep;
    [SerializeField] private float _spawnY;
    [SerializeField] private float _animationDuration;
    
    private List<Target> _targets = new List<Target>();
    private Target _currentTarget;
    private Tween _tween;

    public Target CurrentTarget => _currentTarget;
    
    public event UnityAction IsWin;

    private void OnDisable()
    {
        _currentTarget.IsBreak -= OnTargetBreak;
        _currentTarget.IsTakeHit -= OnTargetTakeHit;
    }

    private void SetCurrentTarget()
    {
        _currentTarget = _targets[0];
        _currentTarget.IsBreak += OnTargetBreak;
        _currentTarget.IsTakeHit += OnTargetTakeHit;
        _progressBar.SpawnProgressBar(_currentTarget.Health);
    }

    private void OnTargetBreak(TargetBase targetBase)
    {
        _currentTarget.IsBreak -= OnTargetBreak;
        _currentTarget.IsTakeHit -= OnTargetTakeHit;
        _progressBar.SubmitHit();
        StartCoroutine(TargetBreakAnimation(targetBase));
    }

    private void OnTargetTakeHit()
    {
        _player.AllowThrow();
        _progressBar.SubmitHit();
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
    
    public void SpawnLevel(Level level)
    {
        for (var i = 0; i < level.TargetCount; i++)
        {
            _targets.Add(Instantiate(_targetTemplate, new Vector3(0f, _spawnY, _spawnStep * (i + 1)), Quaternion.identity, transform));
            _targets[i].SpawnAndSetup(_targetBases[1], 5);
        }
        
        SetCurrentTarget();
    }
}
