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
        Vector3 rotateEuler;
        
        switch (DataManager.GameData.ProgressData.CurrentGamemod)
        {
            case 0:
                rotateEuler = new Vector3(0f, 0f, _rotateDefinitions[_currentIndex].Angle);
                break;
            case 1:
                rotateEuler = new Vector3(0f, 0f, _rotateDefinitions[_currentIndex].Angle);
                break;
            case 2:
                rotateEuler = new Vector3(0f, _rotateDefinitions[_currentIndex].Angle, 0f);
                break;
            default:
                rotateEuler = new Vector3(0f, 0f, _rotateDefinitions[_currentIndex].Angle);
                break;
        }
        
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
        _rotateDefinitions = definitions;
        _currentIndex = 0;
        if (_rotateDefinitions.Count <= 0) return;
        Rotate();
    }
}
