using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _movementPoints;
    [SerializeField] private float _animationDuration;

    private Tween _tween;
    private int _currentPointIndex = 0;

    public void TryMoveCamera(int index)
    {
        if (_currentPointIndex == index) return;

        _currentPointIndex = index;
        _tween = transform.DOMove(_movementPoints[_currentPointIndex].position, _animationDuration);
        _tween = transform.DORotate(_movementPoints[_currentPointIndex].eulerAngles, _animationDuration);
    }
}
