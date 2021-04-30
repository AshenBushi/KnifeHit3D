using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private List<Obstacle> _obstacles;

    private void Awake()
    {
        _obstacles = GetComponentsInChildren<Obstacle>().ToList();
    }

    public void SpawnObstacles(Knife obstacleTemplate, int count)
    {
        for (var i = 0; i < count; i++)
        {
            _obstacles[i].Initialize(obstacleTemplate);
        }
    }
}
