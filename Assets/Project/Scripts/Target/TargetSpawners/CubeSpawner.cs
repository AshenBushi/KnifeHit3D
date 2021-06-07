using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CubeSpawner : TargetSpawner
{
    public override void SpawnLevel()
    {
        var level = LevelManager.CurrentCubeLevel;
        
        TryCleanTargets();
    
        Targets.Add(Instantiate(_template, new Vector3(0f, _spawnY, SpawnZ), Quaternion.identity, transform));
        Targets[0].SpawnTargetBase(null, level);
    }
}
