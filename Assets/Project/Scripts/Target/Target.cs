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
    private int _expReward;

    //Cube Fields
    private Tween _tween;
    private CubeLevel _currentCubeLevel;
    
    public int HitToBreak { get; private set; }

    public event UnityAction<int> IsRotate;
    public event UnityAction IsEdgePass;
    public event UnityAction<TargetBase, int> IsBreak;

    private void Awake()
    {
        _rotator = GetComponent<TargetRotator>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Knife knife)) return;
        SoundManager.PlaySound(SoundNames.TargetHit);
        _targetBase.TakeHit();
        knife.Stuck(_targetBase.transform);
        TakeHit();
    }

    private void TakeHit()
    {
        HitToBreak--;

        if (HitToBreak <= 0)
        {
            TryNextEdge();
        }
    }

    private void SpawnTarget(TargetBase template, int hitToBreak, int experience, int edgeCount, List<RotateDefinition> rotateDefinitions)
    {
        _targetBase = Instantiate(template, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        HitToBreak = hitToBreak;
        _edgeCount = edgeCount;
        _expReward = experience;
        _rotator.StartRotate(rotateDefinitions);
    }
    
    private void SpawnTarget(CubeLevel level, int edgeCount)
    {
        _currentCubeLevel = level;
        _targetBase = Instantiate(level._base, transform.position, Quaternion.Euler(0f, 0f, 0f), transform);
        _edgeCount = edgeCount;
        HitToBreak = level.Cubes[level.Cubes.Count - _edgeCount].HitToBreak;
        _rotator.StartRotate(level.Cubes[level.Cubes.Count - _edgeCount].RotateDefinitions);
        _expReward = level.Cubes[level.Cubes.Count - _edgeCount].Experience;
    }

    public void TryNextEdge()
    {
        _edgeCount--;
            
        if(_edgeCount <= 0)
        {
            SoundManager.PlaySound(SoundNames.TargetBreak);
            IsBreak?.Invoke(Instantiate(_targetBase, _targetBase.transform.position, _targetBase.transform.rotation), _expReward);
            Destroy(gameObject);
        }
        else
        {
            IsRotate?.Invoke(_expReward);
            _tween = _targetBase.transform.DOLocalRotate(_cubeRotatePoints[_currentCubeLevel.Cubes.Count - _edgeCount], 1f);
            _tween.OnComplete(() =>
            {
                HitToBreak = _currentCubeLevel.Cubes[_currentCubeLevel.Cubes.Count - _edgeCount].HitToBreak;
                _expReward = _currentCubeLevel.Cubes[_currentCubeLevel.Cubes.Count - _edgeCount].Experience;
                _rotator.StartRotate(_currentCubeLevel.Cubes[_currentCubeLevel.Cubes.Count - _edgeCount].RotateDefinitions);
                IsEdgePass?.Invoke();
            });
        }
    }
    
    public void SpawnAndSetup(TargetConfig config, Knife obstacleTemplate)
    {
        SpawnTarget(config.Base, config.HitToBreak, config.Experience,1, config.RotateDefinitions);
        
        SetupTargetBase(config, obstacleTemplate);
    }
    
    public void SpawnAndSetup(CubeLevel level, Knife obstacleTemplate)
    {
        SpawnTarget(level, 6);
        
        SetupTargetBase(level, obstacleTemplate);
    }
    
    public void SpawnAndSetup(FlatConfig config, Knife obstacleTemplate)
    {
        SpawnTarget(config.Base, config.HitToBreak, config.Experience,1, config.RotateDefinitions);
        
        SetupTargetBase(config, obstacleTemplate);
    }

    public void SetupTargetBase(TargetConfig config, Knife obstacleTemplate)
    {
        if (config.ObstacleCount != 0) _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
        if (config.HasApple) _targetBase.InitializeApples(0, _appleTemplate);
    }
    
    public void SetupTargetBase(CubeLevel level, Knife obstacleTemplate)
    {
        for (var i = 0; i < level.Cubes.Count; i++)
        {
            if (level.Cubes[i].ObstacleCount != 0) _targetBase.InitializeObstacles(i, level.Cubes[i].ObstacleCount, obstacleTemplate);
            if (level.Cubes[i].HasApple) _targetBase.InitializeApples(i, _appleTemplate);
        }
    }
    
    public void SetupTargetBase(FlatConfig config, Knife obstacleTemplate)
    {
        if (config.ObstacleCount != 0) _targetBase.InitializeObstacles(0, config.ObstacleCount, obstacleTemplate);
        if (config.HasApple) _targetBase.InitializeApples(0, _appleTemplate);
    }
}
