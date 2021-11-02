using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Material _positive;
    [SerializeField] private Material _negative;
    [SerializeField] private RawImage _positiveImage;
    [SerializeField] private RawImage _negativeImage;

    private MeshRenderer _meshRenderer;
    private int _value;
    private WallType _currentWallType;

    public bool IsWallUsed { get; private set; } = true;

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
                ChangeColor(_positiveImage);
                _text.text = $"+{value}";
                break;
            case WallType.Subtraction:
                _meshRenderer.material = _negative;
                ChangeColor(_negativeImage);
                _text.text = $"-{value}";
                break;
            case WallType.Multiplication:
                _meshRenderer.material = _positive;
                ChangeColor(_positiveImage);
                _text.text = $"X{value}";
                break;
            case WallType.Division:
                _meshRenderer.material = _negative;
                ChangeColor(_negativeImage);
                _text.text = $"÷{value}";
                break;
        }
    }

    public void AllowUsing()
    {
        IsWallUsed = true;
    }

    public void DisallowUsing()
    {
        IsWallUsed = false;
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

    private void ChangeColor(RawImage image)
    {
        var color2 = new Color(1f, 1f, 1f, 0f);
        var stockColor = image.color;

        Texture2D texture = new Texture2D(1, 2) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };

        texture.SetPixel(0, 0, stockColor);
        texture.SetPixel(0, 1, Color.Lerp(stockColor, color2, 0.125f));
        texture.SetPixel(0, 2, Color.Lerp(stockColor, color2, 0.250f));
        texture.SetPixel(0, 3, Color.Lerp(stockColor, color2, 0.375f));
        texture.SetPixel(0, 4, Color.Lerp(stockColor, color2, 0.500f));
        texture.SetPixel(0, 5, Color.Lerp(stockColor, color2, 0.625f));
        texture.SetPixel(0, 6, Color.Lerp(stockColor, color2, 0.750f));
        texture.SetPixel(0, 7, Color.Lerp(stockColor, color2, 0.875f));
        texture.SetPixel(0, 8, color2);

        texture.Apply();
        image.texture = texture;
        image.gameObject.SetActive(true);
    }
}

[Serializable]
public enum WallType
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}
