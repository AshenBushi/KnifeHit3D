using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Targets ", menuName = "Target/Create TargetConfig")]
public class TargetConfig : ScriptableObject
{
    [SerializeField] private TargetBase _base;
    [Range(0, 4)]
    [SerializeField] private int _obstacleCount;
    [SerializeField] private bool _haveApple;
    [SerializeField] private int _hitToBreak;
    [SerializeField] private List<RotateDefinition> _rotateDefinitions;

    public TargetBase Base => _base;
    public int ObstacleCount => _obstacleCount;
    public bool HaveApple => _haveApple;
    public int HitToBreak => _hitToBreak;
    public List<RotateDefinition> RotateDefinitions => _rotateDefinitions;
}

[System.Serializable]
public struct RotateDefinition
{
    public float Angle;
    public float Duration;
    public AnimationCurve EaseCurve;
}
