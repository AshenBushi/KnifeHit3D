using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Project.Scripts.Handlers;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    [SerializeField] private TargetHandler _targetHandler;
    [SerializeField] private KnifeHandler _knifeHandler;
    
    private List<ObstacleSpawner> _obstacleSpawners = new List<ObstacleSpawner>();

    private int Gamemod => DataManager.GameData.ProgressData.CurrentGamemod;
    
    private void OnEnable()
    {
        _targetHandler.IsLevelSpawned += OnLevelSpawned;
        KnifeStorage.IsKnifeChanged += OnLevelSpawned;
    }

    private void OnDisable()
    {
        _targetHandler.IsLevelSpawned -= OnLevelSpawned;
        KnifeStorage.IsKnifeChanged -= OnLevelSpawned;
    }

    private void OnLevelSpawned()
    {
        _obstacleSpawners.Clear();
        
        if (Gamemod != 1)
        {
            foreach (var target in _targetHandler.Targets)
            {
                _obstacleSpawners.Add(target.GetComponentInChildren<ObstacleSpawner>());
            }
        }
        else
        {
            if(_targetHandler.Targets.Count > 0)
                _obstacleSpawners = _targetHandler.Targets[0].GetComponentsInChildren<ObstacleSpawner>().ToList();
        }

        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        if(Gamemod != 1)
        {
            for (var i = 0; i < _obstacleSpawners.Count; i++)
            {
                _obstacleSpawners[i].SpawnObstacles(_knifeHandler.CurrentKnifeTemplate,
                    _targetHandler.Targets[i].ObstacleCount[0]);
            }
        }
        else
        {
            for (var i = 0; i < _obstacleSpawners.Count; i++)
            {
                _obstacleSpawners[i].SpawnObstacles(_knifeHandler.CurrentKnifeTemplate,
                    _targetHandler.Targets[0].ObstacleCount[i]);
            }
        }
    }
}
