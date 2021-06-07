using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Handlers;
using UnityEngine;

public class AppleHandler : MonoBehaviour
{
    [SerializeField] private TargetHandler _targetHandler;
    [SerializeField] private SessionHandler _sessionHandler;
    [SerializeField] private AppleCounter _appleCounter;
    [Range(0, 4)]
    [SerializeField] private int[] _indexesWhereSpawn;
    
    private List<AppleSpawner> _appleSpawners = new List<AppleSpawner>();
    private readonly List<Apple> _apples = new List<Apple>();

    private int Gamemod => DataManager.GameData.ProgressData.CurrentGamemod;
    
    public int SlicedAppleCount { get; private set; } = 0;
    
    private void OnEnable()
    {
        _targetHandler.IsLevelSpawned += OnLevelSpawned;
        _sessionHandler.IsSessionStarted += OnSessionStarted;
    }

    private void OnDisable()
    {
        _targetHandler.IsLevelSpawned -= OnLevelSpawned;
        _sessionHandler.IsSessionStarted += OnSessionStarted;
        
        if (_apples is null) return;
        foreach (var apple in _apples)
        {
            apple.IsSliced -= OnAppleSliced;
        }
    }

    private void OnLevelSpawned()
    {
        _appleSpawners.Clear();
        
        if(Gamemod != 1)
        {
            foreach (var target in _targetHandler.Targets)
            {
                _appleSpawners.Add(target.GetComponentInChildren<AppleSpawner>());
            }
        }
        else
        {
            _appleSpawners = _targetHandler.Targets[0].GetComponentsInChildren<AppleSpawner>().ToList();
        }
        
        SpawnApples();
    }
    
    private void OnSessionStarted()
    {
        _appleCounter.gameObject.SetActive(true);
    }

    private void SpawnApples()
    {
        foreach (var spawnerIndex in _indexesWhereSpawn)
        {
            _apples.Add(_appleSpawners[spawnerIndex].SpawnApple());
        }

        foreach (var apple in _apples)
        {
            apple.IsSliced += OnAppleSliced;
        }
    }

    private void OnAppleSliced()
    {
        SlicedAppleCount++;
        _appleCounter.SetCount(SlicedAppleCount);
    }
}
