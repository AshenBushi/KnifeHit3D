using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TargetBase : MonoBehaviour
{
    private const float UpForce = 0;
    private const float Radius = 20;

    private List<ObstacleSpawner> _obstacleSpawners;
    private List<AppleSpawner> _appleSpawners;
    private List<GiftSpawner> _giftSpawners;
    private Tween _tween;

    private void Awake()
    {
        _obstacleSpawners = GetComponentsInChildren<ObstacleSpawner>().ToList();
        _appleSpawners = GetComponentsInChildren<AppleSpawner>().ToList();
        _giftSpawners = GetComponentsInChildren<GiftSpawner>().ToList();
    }

    public void TakeHit()
    {
        var currentPosition = transform.position;

        _tween = transform.DOMove(new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 0.3f), 0.05f).SetLink(gameObject);
        _tween.OnComplete(() =>
        {
            _tween = transform.DOMove(new Vector3(currentPosition.x, currentPosition.y, currentPosition.z), 0.05f).SetLink(gameObject);
        });
    }
    
    public void InitializeObstacles(int spawnerIndex, int count, Knife template)
    {
        _obstacleSpawners[spawnerIndex].SpawnObstacles(template, count);
    }

    public void InitializeApples(int spawnerIndex, Apple template)
    {
        _appleSpawners[spawnerIndex].SpawnApple(template);
    }

    public void Detonate(Vector3 explodePoint, float explosionForce)
    {
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = false;
        }

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
            item.AddExplosionForce(explosionForce, explodePoint, Radius, UpForce);
        }
    }
    
}
