using System.Collections.Generic;
using System.Linq;

public class ObstacleHandler : Singleton<ObstacleHandler>
{
    private List<ObstacleSpawner> _obstacleSpawners = new List<ObstacleSpawner>();

    private int TargetType => TargetHandler.Instance.CurrentSpawnerIndex;
    
    private void OnEnable()
    {
        TargetHandler.Instance.IsLevelSpawned += OnLevelSpawned;
        KnifeStorage.Instance.IsKnifeChanged += OnLevelSpawned;
    }

    private void OnDisable()
    {
        TargetHandler.Instance.IsLevelSpawned -= OnLevelSpawned;
        KnifeStorage.Instance.IsKnifeChanged -= OnLevelSpawned;
    }

    private void OnLevelSpawned()
    {
        _obstacleSpawners.Clear();
        
        if (TargetType != 1)
        {
            foreach (var target in TargetHandler.Instance.Targets)
            {
                _obstacleSpawners.Add(target.GetComponentInChildren<ObstacleSpawner>());
            }
        }
        else
        {
            if(TargetHandler.Instance.Targets.Count > 0)
                _obstacleSpawners = TargetHandler.Instance.Targets[0].GetComponentsInChildren<ObstacleSpawner>().ToList();
        }

        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        if(TargetType != 1)
        {
            for (var i = 0; i < _obstacleSpawners.Count; i++)
            {
                _obstacleSpawners[i].SpawnObstacles(KnifeHandler.Instance.CurrentKnifeTemplate,
                    TargetHandler.Instance.Targets[i].ObstacleCount[0]);
            }
        }
        else
        {
            for (var i = 0; i < _obstacleSpawners.Count; i++)
            {
                _obstacleSpawners[i].SpawnObstacles(KnifeHandler.Instance.CurrentKnifeTemplate,
                    TargetHandler.Instance.Targets[0].ObstacleCount[i]);
            }
        }
    }
}
