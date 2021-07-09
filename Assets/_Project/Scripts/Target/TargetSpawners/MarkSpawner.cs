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
        var colorPreset = LevelManager.GiveColorPreset();
        
        TryCleanTargets();

        for (var i = 0; i < level.Marks.Count; i++)
        {
            Targets.Add(Instantiate(_template, new Vector3(0f, _spawnY, SpawnZ + SpawnStep * i), Quaternion.identity, transform));
            Targets[i].SetupTarget(Color.Lerp(colorPreset.endColor, colorPreset.startColor, (float)i / level.Marks.Count), level.Marks[i]);
        }
    }
}
