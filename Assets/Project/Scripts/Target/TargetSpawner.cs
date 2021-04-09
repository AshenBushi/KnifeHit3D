using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Target _targetTemplate;
    [SerializeField] private float _targetY;
    
    private List<Target> _targets = new List<Target>();

    public void SpawnLevel(Level level)
    {
        for (var i = 0; i < level.TargetCount; i++)
        {
            _targets.Add(Instantiate(_targetTemplate, new Vector3(0f, _targetY, 10f * (i + 1)), Quaternion.identity, transform));
        }
    }
}
