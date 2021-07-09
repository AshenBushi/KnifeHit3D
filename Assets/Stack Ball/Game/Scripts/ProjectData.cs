using UnityEngine;
using Watermelon;

[SetupTab("Settings", texture = "icon_preferences")]
[CreateAssetMenu(fileName = "Project Settings", menuName = "Settings/Project Settings")]
public class ProjectData : ScriptableObject
{
    [Tooltip("Switch camera between ortographic and perspective modes")]
    public bool isOrthographicCamera;
    
    [Header("Combo")]
    [Tooltip("How many platforms should player destroy to start counting combo platforms")]
    public int comboStartColums = 2;
    [Tooltip("Platforms amount required to activate combo state")]
    public int comboRequireColums = 20;
    [Tooltip("Combo state duration in seconds")]
    public float comboActiveTime = 2.5f;

    [Header("Platforms Scale")]
    public float minScaleValue = 0.95f;
    public float maxScaleValue = 1.3f;
    public float scaleStep = 0.03f;

    [Header("Game")]
    public float platformsRotationSpeed = 60;
    public float playerFallTime = 0.05f;
    public float playerSafeTime = 0.05f;
}
