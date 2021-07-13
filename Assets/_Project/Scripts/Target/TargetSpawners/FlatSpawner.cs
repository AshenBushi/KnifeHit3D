using System.Collections;
using UnityEngine;

public class FlatSpawner : TargetSpawner
{
    public override void SpawnLevel()
    {
        var level = LevelManager.CurrentFlatLevel;
        var colorPreset = LevelManager.GiveColorPreset();
        
        TryCleanTargets();
    
        for (var i = 0; i < level.Flats.Count; i++)
        {
            Targets.Add(Instantiate(_template, new Vector3(0f, _spawnY, SpawnZ + SpawnStep * i), Quaternion.identity, transform));
            Targets[i].SetupTarget(Color.Lerp(colorPreset.startColor, colorPreset.endColor, (float)i / level.Flats.Count),null, new CubeLevel(), level.Flats[i]);
        }
    }
}
