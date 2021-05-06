using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetBase : MonoBehaviour
{
    private const float UpForce = 0;
    private const float Radius = 20;
    
    [SerializeField] private float _explosionForce;
    
    private List<ObstacleSpawner> _obstacleSpawners;
    private List<AppleSpawner> _appleSpawners;

    private void Awake()
    {
        _obstacleSpawners = GetComponentsInChildren<ObstacleSpawner>().ToList();
        _appleSpawners = GetComponentsInChildren<AppleSpawner>().ToList();
    }

    public void InitializeObstacles(int spawnerIndex, int count, Knife obstacleTemplate)
    {
        _obstacleSpawners[spawnerIndex].SpawnObstacles(obstacleTemplate, count);
    }

    public void InitializeApples(int spawnerIndex, Apple appleTemplate)
    {
        _appleSpawners[spawnerIndex].SpawnApple(appleTemplate);
    }
    
    public void Detonate(Vector3 explodePoint)
    {
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = false;
        }

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
            item.AddExplosionForce(_explosionForce, explodePoint, Radius, UpForce);
        }
    }
    
}
