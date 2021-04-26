using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Targets ", menuName = "Target/Create CubeConfig")]
public class CubeConfig : ScriptableObject
{
    [Range(0, 4)]
    [SerializeField] private int _obstacleCount;
    [SerializeField] private int _hitToBreak;
    [SerializeField] private List<RotateDefinition> _rotateDefinitions;
    
    public int ObstacleCount => _obstacleCount;
    public int HitToBreak => _hitToBreak;
    public List<RotateDefinition> RotateDefinitions => _rotateDefinitions;
}
