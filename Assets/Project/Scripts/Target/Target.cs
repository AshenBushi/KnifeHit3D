using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetRotator))]
public class Target : MonoBehaviour
{
    [SerializeField] private Apple _appleTemplate;
    [SerializeField] private List<Vector3> _cubeRotatePoints;
    
    private TargetBase _targetBase;
    private TargetRotator _rotator;
    private int _edgeCount;

    //Cube Fields
    private Tween _tween;
    private CubeLevel _currentCubeLevel;
    
    public int HitToBreak { get; private set; }

    public event UnityAction IsRotate;
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
        SoundManager.PlaySound(SoundNames.TargetHit);
        knife.Stuck(_targetBase.transform);
        TakeHit();
    }

    private void TakeHit()
    {
        HitToBreak--;

        IsTakeHit?.Invoke();
        
        if (HitToBreak <= 0)
        {
            TryNextEdge();
        }
    }

    private void SetupTarget(TargetBase template, int hitToBreak, int edgeCount, List<RotateDefinition> rotateDefinitions)
    {
        _targetBase = Instantiate(template, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        HitToBreak = hitToBreak;
        _edgeCount = edgeCount;
        _rotator.StartRotate(rotateDefinitions);
    }
    
    private void SetupTarget(CubeLevel level, int edgeCount)
    {
        _currentCubeLevel = level;
        _targetBase = Instantiate(level._base, transform.position, Quaternion.Euler(0f, 0f, 0f), transform);
        _edgeCount = edgeCount;
        HitToBreak = level.Cubes[level.Cubes.Count - _edgeCount].HitToBreak;
        _rotator.StartRotate(level.Cubes[level.Cubes.Count - _edgeCount].RotateDefinitions);
    }

    private void TryNextEdge()
    {
        _edgeCount--;
            
        if(_edgeCount <= 0)
        {
            SoundManager.PlaySound(SoundNames.TargetBreak);
            IsBreak?.Invoke(Instantiate(_targetBase, _targetBase.transform.position, _targetBase.transform.rotation));
            Destroy(gameObject);
        }
        else
        {
            IsRotate?.Invoke();
            _tween = _targetBase.transform.DOLocalRotate(_cubeRotatePoints[_currentCubeLevel.Cubes.Count - _edgeCount], 1f);
            _tween.OnComplete(() =>
            {
                HitToBreak = _currentCubeLevel.Cubes[_currentCubeLevel.Cubes.Count - _edgeCount].HitToBreak;
                _rotator.StartRotate(_currentCubeLevel.Cubes[_currentCubeLevel.Cubes.Count - _edgeCount].RotateDefinitions);
                IsEdgePass?.Invoke();
            });
        }
    }
    
    public void SpawnAndSetup(TargetConfig config, Knife obstacleTemplate)
    {
        SetupTarget(config.Base, config.HitToBreak, 1, config.RotateDefinitions);
        
        InitializeObstaclesAndApples(config, obstacleTemplate);
    }
    
    public void SpawnAndSetup(CubeLevel level, Knife obstacleTemplate)
    {
        SetupTarget(level, 6);
        
        InitializeObstaclesAndApples(level, obstacleTemplate);
    }
    
    public void SpawnAndSetup(FlatConfig config, Knife obstacleTemplate)
    {
        SetupTarget(config.Base, config.HitToBreak, 1, config.RotateDefinitions);
        
        InitializeObstaclesAndApples(config, obstacleTemplate);
    }

    public void InitializeObstaclesAndApples(TargetConfig config, Knife obstacleTemplate)
    {
        if (config.ObstacleCount != 0) 
            _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
        if (config.HaveApple) 
            _targetBase.InitializeApples(0, _appleTemplate);
    }
    
    public void InitializeObstaclesAndApples(CubeLevel level, Knife obstacleTemplate)
    {
        for (var i = 0; i < level.Cubes.Count; i++)
        {
            if (level.Cubes[i].ObstacleCount != 0) _targetBase.InitializeObstacles(i, level.Cubes[i].ObstacleCount, obstacleTemplate);
            if (level.Cubes[i].IsApple) _targetBase.InitializeApples(i, _appleTemplate);
        }
    }
    
    public void InitializeObstaclesAndApples(FlatConfig config, Knife obstacleTemplate)
    {
        if (config.ObstacleCount != 0) _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
        if (config.IsApple) _targetBase.InitializeApples(0, _appleTemplate);
    }
}
