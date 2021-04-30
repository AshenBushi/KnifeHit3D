using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetRotator))]
public class Target : MonoBehaviour
{
    [SerializeField] private List<Vector3> _cubeRotatePoints;
    
    private TargetBase _targetBase;
    private TargetRotator _rotator;
    private int _edgeCount;
    private int _hitToBreak;
    //Cube Fields
    private Tween _tween;
    private CubeLevel _cubeLevel;
    
    public int HitToBreak => _hitToBreak;

    public event UnityAction IsTakeHit;
    public event UnityAction IsEdgePass;
    public event UnityAction<TargetBase> IsBreak;

    private void Awake()
    {
        _rotator = GetComponent<TargetRotator>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Knife knife)) return;
        knife.Stuck(_targetBase.transform);
        TakeHit();
    }

    private void TakeHit()
    {
        _hitToBreak--;

        if (_hitToBreak <= 0)
        {
            _edgeCount--;
            
            if(_edgeCount <= 0)
            {
                Break();
            }
            else
            {
                _tween = _targetBase.transform.DOLocalRotate(_cubeRotatePoints[_cubeLevel.Cubes.Count - _edgeCount], 1f);
                _tween.OnComplete(() =>
                {
                    LoadCubeSettings();
                    IsEdgePass?.Invoke();
                });

            }
        }
        else
        {
            IsTakeHit?.Invoke();
        }
    }

    private void Break()
    {
        IsBreak?.Invoke(Instantiate(_targetBase, _targetBase.transform.position, _targetBase.transform.rotation));
        Destroy(gameObject);
    }

    private void LoadCubeSettings()
    {
        _hitToBreak = _cubeLevel.Cubes[_cubeLevel.Cubes.Count - _edgeCount].HitToBreak;
        _rotator.StartRotate(_cubeLevel.Cubes[_cubeLevel.Cubes.Count - _edgeCount].RotateDefinitions);
    }
    
    public void SpawnAndSetup(TargetConfig config, Knife obstacleTemplate)
    {
        _targetBase = Instantiate(config.Base, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        _edgeCount = 1;
        _hitToBreak = config.HitToBreak;
        _rotator.StartRotate(config.RotateDefinitions);
        if (config.ObstacleCount <= 0) return;
        _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
    }
    
    public void SpawnAndSetup(CubeLevel level, Knife obstacleTemplate)
    {
        _cubeLevel = level;
        _targetBase = Instantiate(_cubeLevel.Base, transform.position, Quaternion.Euler(0f, 0f, 0f), transform);
        _edgeCount = _cubeLevel.Cubes.Count;

        for (var i = 0; i < _cubeLevel.Cubes.Count; i++)
        {
            _targetBase.InitializeObstacles(i, _cubeLevel.Cubes[i].ObstacleCount, obstacleTemplate);
        }
        
        LoadCubeSettings();
    }
    
    public void SpawnAndSetup(FlatConfig config, Knife obstacleTemplate)
    {
        _targetBase = Instantiate(config.Base, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        _edgeCount = 1;
        _hitToBreak = config.HitToBreak;
        _rotator.StartRotate(config.RotateDefinitions);
        if (config.ObstacleCount <= 0) return;
        _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
    }

    public void ReinitializeObstacles(TargetConfig config, Knife obstacleTemplate)
    {
        _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
    }
    
    public void ReinitializeObstacles(CubeLevel level, Knife obstacleTemplate)
    {
        for (var i = 0; i < level.Cubes.Count; i++)
        {
            _targetBase.InitializeObstacles(i, level.Cubes[i].ObstacleCount, obstacleTemplate);
        }
    }
    
    public void ReinitializeObstacles(FlatConfig config, Knife obstacleTemplate)
    {
        _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
    }
}
