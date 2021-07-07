﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TargetBase : MonoBehaviour
{
    private const float UpForce = 0;
    private const float Radius = 20;
    
    private Tween _tween;

    public void SpringBack()
    {
        var currentPosition = transform.position;

        _tween = transform.DOMove(new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 0.3f), 0.05f).SetLink(gameObject);
        _tween.OnComplete(() =>
        {
            _tween = transform.DOMove(new Vector3(currentPosition.x, currentPosition.y, currentPosition.z), 0.05f).SetLink(gameObject);
        });
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