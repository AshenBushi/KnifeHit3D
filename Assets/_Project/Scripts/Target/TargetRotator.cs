using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TargetRotator : MonoBehaviour
{
    private Tween _rotator;
    private List<RotateDefinition> _rotateDefinitions = new List<RotateDefinition>();
    private int _currentIndex;

    private void Rotate()
    {
        var rotateEuler = (int)GamemodHandler.CurrentGamemod switch
        {
            0 => new Vector3(0f, 0f, _rotateDefinitions[_currentIndex].Angle),
            1 => new Vector3(0f, 0f, _rotateDefinitions[_currentIndex].Angle),
            2 => new Vector3(0f, _rotateDefinitions[_currentIndex].Angle, 0f),
            _ => new Vector3(0f, 0f, _rotateDefinitions[_currentIndex].Angle)
        };

        _rotator = transform.DORotate(transform.eulerAngles + rotateEuler, _rotateDefinitions[_currentIndex].Duration, RotateMode.FastBeyond360)
            .SetEase(_rotateDefinitions[_currentIndex].EaseCurve).SetLink(gameObject);
        _rotator.OnComplete(() =>
        {
            _currentIndex++;
            
            if (_currentIndex >= _rotateDefinitions.Count)
                _currentIndex = 0;

            Rotate();
        });
    }

    public void StartRotate(List<RotateDefinition> definitions)
    {
        _rotator.Kill();
        _rotateDefinitions = definitions;
        _currentIndex = 0;
        if (_rotateDefinitions.Count <= 0) return;
        Rotate();
    }
}
