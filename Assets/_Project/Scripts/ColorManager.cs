using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorManager : Singleton<ColorManager>
{
    [SerializeField] private List<LevelColorPreset> _levelColorPresets;

    private Texture2D _texture;
    
    public LevelColorPreset CurrentColorPreset { get; private set; }
    public event UnityAction IsPresetChanged;

    protected override void Awake()
    {
        base.Awake();
        
        _texture = new Texture2D(1, 9) {wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear};
    }

    public void RandomColorPreset()
    {
        CurrentColorPreset = _levelColorPresets[Random.Range(0, _levelColorPresets.Count)];
        
        IsPresetChanged?.Invoke();
    }

    public void ColorizeImage(RawImage image)
    {
        var color1 = CurrentColorPreset.cameraStartColor;
        var color2 = CurrentColorPreset.cameraEndColor;

        _texture.SetPixel(0, 0, color1);
        _texture.SetPixel(0, 1, Color.Lerp(color1, color2, 0.125f));
        _texture.SetPixel(0, 2, Color.Lerp(color1, color2, 0.250f));
        _texture.SetPixel(0, 3, Color.Lerp(color1, color2, 0.375f));
        _texture.SetPixel(0, 4, Color.Lerp(color1, color2, 0.500f));
        _texture.SetPixel(0, 5, Color.Lerp(color1, color2, 0.625f));
        _texture.SetPixel(0, 6, Color.Lerp(color1, color2, 0.750f));
        _texture.SetPixel(0, 7, Color.Lerp(color1, color2, 0.875f));
        _texture.SetPixel(0, 8, color2);

        _texture.Apply();
        image.texture = _texture;
    }
}
