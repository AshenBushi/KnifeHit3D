using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetRotator))]
public class Target : MonoBehaviour
{
    private TargetBase _targetBase;
    private TargetRotator _rotator;
    private int _hitToBreak;
    
    public int HitToBreak => _hitToBreak;

    public event UnityAction IsTakeHit;
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
        
        if(_hitToBreak <= 0)
            Break();
        else
            IsTakeHit?.Invoke();
    }

    private void Break()
    {
        IsBreak?.Invoke(Instantiate(_targetBase, _targetBase.transform.position, _targetBase.transform.rotation));
        Destroy(gameObject);
    }
    
    public void SpawnAndSetup(TargetConfig config, Knife obstacleTemplate)
    {
        _targetBase = Instantiate(config.Base, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        _hitToBreak = config.HitToBreak;
        _rotator.StartRotate(config.RotateDefinitions);
        if (config.ObstacleCount <= 0) return;
        _targetBase.InitializeObstacles(config.ObstacleCount, obstacleTemplate);
    }

    public void ReinitializeObstacle(TargetConfig config, Knife obstacleTemplate)
    {
        _targetBase.InitializeObstacles(config.ObstacleCount, obstacleTemplate);
    }
}
