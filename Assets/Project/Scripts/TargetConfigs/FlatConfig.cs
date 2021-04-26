using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Targets ", menuName = "Target/Create FlatConfig")]
public class FlatConfig : ScriptableObject
{
    [SerializeField] private TargetBase _base;
    [Range(0, 4)]
    [SerializeField] private int _obstacleCount;
    [SerializeField] private int _hitToBreak;
    [SerializeField] private List<RotateDefinition> _rotateDefinitions;

    public TargetBase Base => _base;
    public int ObstacleCount => _obstacleCount;
    public int HitToBreak => _hitToBreak;
    public List<RotateDefinition> RotateDefinitions => _rotateDefinitions;
}
