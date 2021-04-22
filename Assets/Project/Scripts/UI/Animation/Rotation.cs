using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Vector3 _defaultRotation;
    
    private Tween _tween;
    
    private void OnEnable()
    {
        Rotate();
    }

    private void OnDisable()
    {
        transform.localRotation = Quaternion.Euler(_defaultRotation);
        _tween.Kill();
    }

    private void Rotate()
    {
        _tween = transform.DORotate(_direction, _duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLink(gameObject);
        _tween.OnComplete(Rotate);
    }

    public void ChangeDirection(Vector3 direction, Vector3 defaultRotation)
    {
        _direction = direction;
        _defaultRotation = defaultRotation;
    }
}
