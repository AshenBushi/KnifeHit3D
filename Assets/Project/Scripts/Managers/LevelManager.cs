using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Level
{
    public List<TargetConfig> Targets;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;

    private Level _currentLevel;
    private int _currentIndex = 0;
    
    public Level CurrentLevel => _currentLevel;

    public int CurrentIndex => _currentIndex;

    private void Awake()
    {
        _currentLevel = _levels[_currentIndex];
    }
}
