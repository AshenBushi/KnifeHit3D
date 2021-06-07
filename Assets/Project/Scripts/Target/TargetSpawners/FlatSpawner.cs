using System.Collections;
using UnityEngine;

public class FlatSpawner : TargetSpawner
{
    public override void SpawnLevel()
    {
        var level = LevelManager.CurrentFlatLevel;
        
        TryCleanTargets();
    
        for (var i = 0; i < level.Flats.Count; i++)
        {
            Targets.Add(Instantiate(_template, new Vector3(0f, _spawnY, SpawnZ + SpawnStep * i), Quaternion.identity, transform));
            Targets[i].SpawnTargetBase(null, new CubeLevel(), level.Flats[i]);
        }
    }
}
