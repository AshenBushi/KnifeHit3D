using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Watermelon;
using Tween = DG.Tweening.Tween;

public class TargetBase : MonoBehaviour
{
    private const float UpForce = 0;
    private const float Radius = 20;
    
    private Tween _tween;

    private IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(5f);
        
        Destroy(gameObject);
    }
    
    public void SpringBack()
    {
        var currentPosition = transform.position;
        
        Vibration.Vibrate(20);

        _tween = ShortcutExtensions.DOMove(transform, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 0.3f), 0.05f).SetLink(gameObject);
        _tween.OnComplete(() =>
        {
            _tween = ShortcutExtensions.DOMove(transform, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z), 0.05f).SetLink(gameObject);
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

        StartCoroutine(SelfDestruction());
    }

    public void SetColor(Color color)
    {
        var pieces = GetComponentsInChildren<MeshRenderer>();

        foreach (var piece in pieces)
        {
            piece.material.color = color;
        }
    }
    
}
