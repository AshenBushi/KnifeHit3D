using System;
using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Material _positive;
    [SerializeField] private Material _negative;
    
    private MeshRenderer _meshRenderer;
    private int _value;
    private WallType _currentWallType;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Init(WallType type, int value)
    {
        _currentWallType = type;
        _value = value;

        switch (type)
        {
            case WallType.Addition:
                _meshRenderer.material = _positive;
                _text.text = $"+{value}";
                break;
            case WallType.Subtraction:
                _meshRenderer.material = _negative;
                _text.text = $"-{value}";
                break;
            case WallType.Multiplication:
                _meshRenderer.material = _positive;
                _text.text = $"X{value}";
                break;
            case WallType.Division:
                _meshRenderer.material = _negative;
                _text.text = $"÷{value}";
                break;
        }
    }

    public int ChangeValue(int startValue)
    {
        var endValue = _currentWallType switch
        {
            WallType.Addition => startValue + _value,
            WallType.Subtraction => startValue - _value,
            WallType.Multiplication => startValue * _value,
            WallType.Division => startValue / _value,
            _ => startValue
        };

        return endValue;
    }
}

public enum WallType
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}
