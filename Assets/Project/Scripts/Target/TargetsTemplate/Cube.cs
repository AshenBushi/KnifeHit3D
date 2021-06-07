using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Cube : Target
{
    private readonly float _explosionForce = 1500f;
    private readonly Vector3 _explosionPosition = new Vector3(0f, 0f, 10f);
    private readonly int _maxEdgeCount = 6;

    private CubeLevel _currentCubeLevel;
    
    public override event UnityAction<int> IsTargetBreak;
    public override event UnityAction<int, int> IsEdgePass;
    
    private void Awake()
    {
        Rotator = GetComponent<TargetRotator>();
    }
    
    private void SetNextEdge()
    {
        EdgeCount--;
        HitToBreak = _currentCubeLevel.Cubes[_maxEdgeCount - EdgeCount].HitToBreak;
        Rotator.StartRotate(_currentCubeLevel.Cubes[_maxEdgeCount - EdgeCount].RotateDefinitions);
        ExpReward = _currentCubeLevel.Cubes[_maxEdgeCount - EdgeCount].Experience;
    }

    public override void BreakTarget()
    {
        EdgeCount--;

        if (EdgeCount <= 0)
        {
            SoundManager.PlaySound(SoundNames.TargetBreak);
            var targetBase = Instantiate(Base, Base.transform.position, Base.transform.rotation);
            targetBase.Detonate(_explosionPosition, _explosionForce);
            IsTargetBreak?.Invoke(ExpReward);
            Destroy(gameObject);
        }
        else
        {
            SetNextEdge();
            IsEdgePass?.Invoke(ExpReward, _maxEdgeCount - EdgeCount);
        }
    }
    
    public override void SpawnTargetBase(MarkConfig markConfig = null, CubeLevel level = new CubeLevel(), FlatConfig flatConfig = null)
    {
        if(level._base == null) return;
        _currentCubeLevel = level;
        Base = Instantiate(_currentCubeLevel._base, transform.position, Quaternion.Euler(0f, 0f, 0f), transform);
        EdgeCount = _maxEdgeCount;
        HitToBreak = _currentCubeLevel.Cubes[0].HitToBreak;
        for (var i = 0; i < _currentCubeLevel.Cubes.Count; i++)
        {
            ObstacleCount[i] = _currentCubeLevel.Cubes[i].ObstacleCount;
        }
        Rotator.StartRotate(_currentCubeLevel.Cubes[0].RotateDefinitions);
        ExpReward = _currentCubeLevel.Cubes[0].Experience;
    }
}