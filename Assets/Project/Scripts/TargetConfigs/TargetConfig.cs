using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Targets ", menuName = "Target/Create TargetConfig")]
public class TargetConfig : ScriptableObject
{
    [SerializeField] private TargetBase _base;
    [Range(0, 4)]
    [SerializeField] private int _obstacleCount;
    [SerializeField] private bool _hasApple;
    [SerializeField] private bool _hasGift;
    [SerializeField] private int _hitToBreak;
    [SerializeField] private int _experience;
    [SerializeField] private List<RotateDefinition> _rotateDefinitions;

    public TargetBase Base => _base;
    public int ObstacleCount => _obstacleCount;
    public bool HasApple => _hasApple;
    public bool HasGift => _hasGift;
    public int HitToBreak => _hitToBreak;
    public int Experience => _experience;
    public List<RotateDefinition> RotateDefinitions => _rotateDefinitions;
}

[System.Serializable]
public struct RotateDefinition
{
    public float Angle;
    public float Duration;
    public AnimationCurve EaseCurve;
}
