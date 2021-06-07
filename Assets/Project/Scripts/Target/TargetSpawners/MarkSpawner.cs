using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MarkSpawner : TargetSpawner
{
    public override void SpawnLevel()
    {
        var level = LevelManager.CurrentMarkLevel;

        TryCleanTargets();

        for (var i = 0; i < level.Marks.Count; i++)
        {
            Targets.Add(Instantiate(_template, new Vector3(0f, _spawnY, SpawnZ + SpawnStep * i), Quaternion.identity, transform));
            Targets[i].SpawnTargetBase(level.Marks[i]);
        }
    }
}
