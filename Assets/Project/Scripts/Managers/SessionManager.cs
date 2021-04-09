using System;
using System.Collections;
using UnityEngine;

public struct Level
{
    public int TargetCount;
}

public class SessionManager : MonoBehaviour
{
    [SerializeField] private KnifeSpawner _knifeSpawner;
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private ScoreHandler _scoreHandler;
    [SerializeField] private LoseScreen _loseScreen;

    private readonly Level _currentLevel = new Level() {TargetCount = 5};

    private void OnEnable()
    {
        _knifeSpawner.IsStuck += OnStuck;
        _knifeSpawner.IsLose += OnLose;
    }

    private void OnDisable()
    {
        _knifeSpawner.IsStuck -= OnStuck;
        _knifeSpawner.IsLose -= OnLose;
    }

    private void Start()
    {
        _targetSpawner.SpawnLevel(_currentLevel);
    }

    private void OnStuck()
    {
        _scoreHandler.AddScore();
    }
    
    private void OnLose()
    {
        _loseScreen.Lose();
    }
}
