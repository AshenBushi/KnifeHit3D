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
    [SerializeField] private float _spawnZ;
    [SerializeField] private float _spawnStep;
    [SerializeField] private float _animationDuration;
    [Header("Target Settings")]
    [SerializeField] private Target _targetTemplate;
    [SerializeField] private float _targetSpawnY;
    [SerializeField] private Vector3 _targetExplodePosition;
    [Header("Cube Settings")]
    [SerializeField] private Target _cubeTemplate;
    [SerializeField] private float _cubeSpawnY;
    [SerializeField] private Vector3 _cubeExplodePosition;
    [Header("Flat Settings")]
    [SerializeField] private Target _flatTemplate;
    [SerializeField] private float _flatSpawnY;
    [SerializeField] private Vector3 _flatExplodePosition;

    
    private List<Target> _targets = new List<Target>();
    private Target _currentTarget;
    private Tween _tween;

    public event UnityAction IsWin;

    private void OnDisable()
    {
        if (_currentTarget == null) return;
        _currentTarget.IsRotate -= OnRotate;
        _currentTarget.IsBreak -= OnTargetBreak;
        _currentTarget.IsTakeHit -= OnTargetTakeHit;
        _currentTarget.IsEdgePass -= OnEdgePass;
    }

    private void OnTargetBreak(TargetBase targetBase)
    {
        _currentTarget.IsRotate -= OnRotate;
        _currentTarget.IsBreak -= OnTargetBreak;
        _currentTarget.IsTakeHit -= OnTargetTakeHit;
        _currentTarget.IsEdgePass -= OnEdgePass;
        StartCoroutine(TargetBreakAnimation(targetBase));
    }

    private void OnTargetTakeHit()
    {
        _hitScoreDisplayer.SubmitHit();
    }

    private void OnEdgePass()
    {
        _player.AllowThrow();
        _levelProgressDisplayer.NextPoint();
        _hitScoreDisplayer.SpawnHitScores(_currentTarget.HitToBreak);
    }

    private void OnRotate()
    {
        _player.DisallowThrow();
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
        _player.DisallowThrow();
        
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                targetBase.Detonate(_targetExplodePosition);
                break;
            case 1:
                targetBase.Detonate(_cubeExplodePosition);
                break;
            case 2:
                targetBase.Detonate(_flatExplodePosition);
                break;
            default:
                targetBase.Detonate(_targetExplodePosition);
                break;
        }
        
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

    private void TryClearTargets()
    {
        if (_targets.Count < 1) return;
        foreach (var target in _targets)
        {
            Destroy(target.gameObject);
        }

        _targets.Clear();
    }
    
    public void SetCurrentTarget()
    {
        _currentTarget = _targets[0];
        _currentTarget.IsRotate += OnRotate;
        _currentTarget.IsBreak += OnTargetBreak;
        _currentTarget.IsTakeHit += OnTargetTakeHit;
        _currentTarget.IsEdgePass += OnEdgePass;
        _hitScoreDisplayer.SpawnHitScores(_currentTarget.HitToBreak);
    }

    public void SpawnLevel(TargetLevel targetLevel, Knife obstacleTemplate)
    {
        TryClearTargets();
        
        for (var i = 0; i < targetLevel.Targets.Count; i++)
        {
            _targets.Add(Instantiate(_targetTemplate, new Vector3(0f, _targetSpawnY, _spawnZ + _spawnStep * i), Quaternion.identity, transform));
            _targets[i].SpawnAndSetup(targetLevel.Targets[i], obstacleTemplate);
        }
    }
    
    public void SpawnLevel(CubeLevel cubeLevel, Knife obstacleTemplate)
    {
        TryClearTargets();
        
        _targets.Add(Instantiate(_cubeTemplate, new Vector3(0f, _cubeSpawnY, _spawnZ), Quaternion.identity,
            transform));
        _targets[0].SpawnAndSetup(cubeLevel, obstacleTemplate);
    }
    
    public void SpawnLevel(FlatLevel flatLevel, Knife obstacleTemplate)
    {
        TryClearTargets();
        
        for (var i = 0; i < flatLevel.Flats.Count; i++)
        {
            _targets.Add(Instantiate(_flatTemplate, new Vector3(0f, _flatSpawnY, _spawnZ + _spawnStep * i), Quaternion.identity, transform));
            _targets[i].SpawnAndSetup(flatLevel.Flats[i], obstacleTemplate);
        }
    }

    public void Reload(TargetLevel targetLevel, Knife obstacleTemplate)
    {
        for (var i = 0; i < targetLevel.Targets.Count; i++)
        {
            _targets[i].InitializeObstaclesAndApples(targetLevel.Targets[i], obstacleTemplate);
        }
    }
    
    public void Reload(CubeLevel cubeLevel, Knife obstacleTemplate)
    {
        _targets[0].InitializeObstaclesAndApples(cubeLevel, obstacleTemplate);
    }
    
    public void Reload(FlatLevel flatLevel, Knife obstacleTemplate)
    {
        for (var i = 0; i < flatLevel.Flats.Count; i++)
        {
            _targets[i].InitializeObstaclesAndApples(flatLevel.Flats[i], obstacleTemplate);
        }
    }
}
