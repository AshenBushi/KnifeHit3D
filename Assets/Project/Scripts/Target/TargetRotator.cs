using System;
using DG.Tweening;
using UnityEngine;

public class TargetRotator : MonoBehaviour
{
    [SerializeField] private float _duration;

    private Tween _rotator;

    private void Start()
    {
        Rotate();
    }

    private void Rotate()
    {
        _rotator = transform.DORotate(new Vector3(0f, 0f, 360f), _duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLink(gameObject);
        _rotator.OnComplete(() =>
        {
            Rotate();
        });
    }

    public void Kill()
    {
        _rotator.Kill();
    }
}
