using System.Collections.Generic;
using UnityEngine;

public class TargetBase : MonoBehaviour
{
    private const float UpForce = 0;
    private const float Radius = 20;
    
    [SerializeField] private List<TargetObstacle> _obstacles;
    [SerializeField] private float _explosionForce;
    [SerializeField] private Transform _explodePoint;

    public void InitializeObstacles(int count, Knife obstacleTemplate)
    {
        for (var i = 0; i < count; i++)
        {
            _obstacles[i].Initialize(obstacleTemplate);
        }
    }
    
    public void Detonate()
    {
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = false;
        }

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
            item.AddExplosionForce(_explosionForce, _explodePoint.position, Radius, UpForce);
        }
    }
    
}
