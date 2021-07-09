using UnityEngine;
using Watermelon;

[CreateAssetMenu(fileName = "Color Preset", menuName = "Content/Color Preset")]
public class LevelColorPreset : ScriptableObject
{
    public Color startColor = Color.white;
    public Color endColor = Color.white;

    [LineSpacer("Background")]
    public Color cameraStartColor = Color.white;
    public Color cameraEndColor = Color.white;
}
